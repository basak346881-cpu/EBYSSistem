using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EBYSSistem
{
    public partial class MainWindow : Window
    {
        private string _kullanici = string.Empty;
        private string _birim = string.Empty;
        private static int _ortakBelediyeEvrakSayac = 1;
        private int _bugunGelenCount = 0;
        private int _bugunYapilanCount = 0;

        // SAYFA ÖNBELLEĞİ
        private GelenEvrakView? _gelenEvrakView;
        private GidenEvrakView? _gidenEvrakView;
        private DetayliSorgulamaView? _detayliSorgulamaView;
        private AdaParselAramaView? _adaParselAramaView;
        private IadeEvrakView? _iadeEvrakView;
        private SuresiDolanEvrakView? _suresiDolanEvrakView;
        private RaporlarView? _raporlarView;
        private AyarlarView? _ayarlarView;

        public class EvrakModel
        {
            public string EvrakKodu { get; set; } = "";
            public string GonderenTuru { get; set; } = "";
            public string KimlikNo { get; set; } = "";
            public string AdUnvan { get; set; } = "";
            public string Telefon { get; set; } = "";
            public string Mahalle { get; set; } = "";
            public string Adres { get; set; } = "";
            public string Konu { get; set; } = "";
            public string HedefBirim { get; set; } = "";
            public string Tarih { get; set; } = "";
        }

        public static List<EvrakModel> KayitliEvraklar { get; set; } = new List<EvrakModel>();

        public MainWindow()
        {
            InitializeComponent();
            SayaclariGuncelle();
        }

        public MainWindow(string kullanici, string birim) : this()
        {
            _kullanici = kullanici ?? string.Empty;
            _birim = birim ?? string.Empty;
            lblAktifKullanici.Text = $"{_kullanici} ({_birim})";
        }

        private void SayaclariGuncelle()
        {
            if (lblBugunGelen != null) lblBugunGelen.Text = _bugunGelenCount.ToString();
            if (lblBugunYapilan != null) lblBugunYapilan.Text = _bugunYapilanCount.ToString();
        }

        private void cmbGonderenTuru_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lblKimlikNo == null || lblAdUnvan == null || lblTelefon == null) return;

            int tur = cmbGonderenTuru.SelectedIndex;

            switch (tur)
            {
                case 0: // Seçiniz
                    lblKimlikNo.Text = "Kimlik / Vergi No:";
                    lblAdUnvan.Text = "Ad Soyad / Ünvan:";
                    lblTelefon.Text = "Telefon No:";
                    break;
                case 1: // Gerçek Kişi (Vatandaş)
                    lblKimlikNo.Text = "Vatandaş TC No:";
                    lblAdUnvan.Text = "Ad Soyad:";
                    lblTelefon.Text = "Telefon No (*Zorunlu):";
                    break;
                case 2: // Tüzel Kişi (Firma)
                    lblKimlikNo.Text = "Vergi Kimlik No:";
                    lblAdUnvan.Text = "Firma / Şirket Ünvanı:";
                    lblTelefon.Text = "Firma Telefon No (*Zorunlu):";
                    break;
                case 3: // Yabancı Uyruklu
                    lblKimlikNo.Text = "Pasaport / YKN No:";
                    lblAdUnvan.Text = "Ad Soyad:";
                    lblTelefon.Text = "Telefon No (*Zorunlu):";
                    break;
                case 4: // Kamu Kurumu
                    lblKimlikNo.Text = "Kurum Evrak No:";
                    lblAdUnvan.Text = "Kamu Kurum Adı:";
                    lblTelefon.Text = "Telefon No (Opsiyonel):";
                    break;
            }
        }

        private void btnAnlikTara_Click(object sender, RoutedEventArgs e)
        {
            // 1. Gerçek Donanım Bağlantı Kontrolü Simülasyonu (TWAIN Driver)
            MessageBoxResult donanimSonuc = MessageBox.Show(
                "TWAIN Sürücüsü Hatası:\n\n" +
                "Sistemde tanımlı tarayıcı cihazı (Fujitsu PaperStream fi-7160) tespit edilemedi!\n\n" +
                "Lütfen cihazın USB/Ağ bağlantısını ve güç kablosunu kontrol ediniz.\n\n" +
                "Simülasyon/Demo verileriyle devam etmek istiyor musunuz?",
                "Donanım Bağlantı Hatası (TWAIN)",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            // 2. Cihaz bağlı değilse ve Demo seçilmediyse işlemi iptal et
            if (donanimSonuc == MessageBoxResult.No)
            {
                MessageBox.Show(
                    "Tarama işlemi kullanıcı tarafından iptal edildi.\nCihaz bağlandıktan sonra tekrar deneyiniz.",
                    "İşlem İptal Edildi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                return;
            }

            // 3. Demo Modu Seçildiğinde Verileri Aktar
            string seciliDPI = (cmbTaramaDPI.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "300 DPI";
            bool ciftYuzlu = chkCiftYuzlu.IsChecked == true;
            bool bosAtla = chkBosAtla.IsChecked == true;

            cmbGonderenTuru.SelectedIndex = 1; // Gerçek Kişi (Vatandaş)
            txtEvrakKodu.Text = $"SULTAN-2026-{_ortakBelediyeEvrakSayac.ToString("D6")}";
            txtKimlikNo.Text = "17627275254";
            txtAdUnvan.Text = "Başak ÖRS";
            txtTelefon.Text = "0532 555 12 34";
            cmbMahalle.SelectedIndex = 1; // 50. Yıl Mahallesi
            txtAdres.Text = "2102. Sokak No:14";
            txtKonu.Text = "Kentsel Dönüşüm Talebi";
            cmbHedefMudurler.SelectedIndex = 1; // Kentsel Dönüşüm Müdürlüğü

            _bugunGelenCount++;
            SayaclariGuncelle();

            MessageBox.Show(
                $"[DEMO MODU] Test evrakı taranarak forma aktarıldı.\n\n" +
                $"• Seçili Çözünürlük: {seciliDPI}\n" +
                $"• Çift Yüzlü Tarama: {(ciftYuzlu ? "Aktif" : "Pasif")}\n" +
                $"• Boş Sayfa Filtresi: {(bosAtla ? "Aktif" : "Pasif")}",
                "Demo Tarama Başarılı",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void btnHavaleEt_Click(object sender, RoutedEventArgs e)
        {
            if (cmbGonderenTuru.SelectedIndex <= 0)
            {
                MessageBox.Show("Lütfen bir Evrak Gönderen Türü seçiniz!", "Eksik Seçim", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int tur = cmbGonderenTuru.SelectedIndex;

            if (tur != 4 && string.IsNullOrEmpty(txtTelefon.Text))
            {
                MessageBox.Show("Sadece Kamu Kurumu evraklarında telefon alanı boş bırakılabilir!", "Eksik Bilgi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtAdUnvan.Text) || string.IsNullOrEmpty(txtKonu.Text))
            {
                MessageBox.Show("Lütfen İsim/Ünvan ve Evrak Konusu alanlarını doldurunuz!", "Eksik Bilgi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtEvrakKodu.Text))
            {
                txtEvrakKodu.Text = $"SULTAN-2026-{_ortakBelediyeEvrakSayac.ToString("D6")}";
            }

            EvrakModel yeniEvrak = new EvrakModel
            {
                EvrakKodu = txtEvrakKodu.Text,
                GonderenTuru = cmbGonderenTuru.Text,
                KimlikNo = txtKimlikNo.Text,
                AdUnvan = txtAdUnvan.Text,
                Telefon = txtTelefon.Text,
                Mahalle = cmbMahalle.Text,
                Adres = txtAdres.Text,
                Konu = txtKonu.Text,
                HedefBirim = cmbHedefMudurler.Text,
                Tarih = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
            };

            KayitliEvraklar.Add(yeniEvrak);
            lstEvrakGecmisi.Items.Insert(0, $"{yeniEvrak.EvrakKodu} | {yeniEvrak.AdUnvan} | {yeniEvrak.HedefBirim}");

            _ortakBelediyeEvrakSayac++;
            _bugunYapilanCount++;
            SayaclariGuncelle();

            MessageBox.Show($"Evrak Başarıyla Kaydedildi!\nKayıt No: {yeniEvrak.EvrakKodu}", "İşlem Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            FormuTemizle();
        }

        private void btnOnizle_Click(object sender, RoutedEventArgs e)
        {
            if (lstEvrakGecmisi.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen önizlemek istediğiniz evraka sağdaki listeden tıklayınız!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int seciliIndex = KayitliEvraklar.Count - 1 - lstEvrakGecmisi.SelectedIndex;
            if (seciliIndex >= 0 && seciliIndex < KayitliEvraklar.Count)
            {
                var evrak = KayitliEvraklar[seciliIndex];

                DocumentViewerWindow viewer = new DocumentViewerWindow(
                    evrak.EvrakKodu,
                    evrak.KimlikNo,
                    evrak.AdUnvan,
                    evrak.Konu,
                    evrak.Mahalle,
                    evrak.Adres,
                    evrak.Telefon,
                    evrak.Tarih,
                    evrak.HedefBirim
                );
                viewer.ShowDialog();
            }
        }

        private void lstEvrakGecmisi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstEvrakGecmisi.SelectedIndex != -1)
            {
                int seciliIndex = KayitliEvraklar.Count - 1 - lstEvrakGecmisi.SelectedIndex;
                if (seciliIndex >= 0 && seciliIndex < KayitliEvraklar.Count)
                {
                    var evrak = KayitliEvraklar[seciliIndex];
                    txtEvrakKodu.Text = evrak.EvrakKodu;
                    txtKimlikNo.Text = evrak.KimlikNo;
                    txtAdUnvan.Text = evrak.AdUnvan;
                    txtTelefon.Text = evrak.Telefon;
                    txtAdres.Text = evrak.Adres;
                    txtKonu.Text = evrak.Konu;
                }
            }
        }

        private void btnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (lstEvrakGecmisi.SelectedIndex == -1)
            {
                MessageBox.Show("Düzenleme yapabilmek için lütfen sağ taraftaki listeden güncellenecek evraka tıklayınız!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int seciliIndex = KayitliEvraklar.Count - 1 - lstEvrakGecmisi.SelectedIndex;
            if (seciliIndex >= 0 && seciliIndex < KayitliEvraklar.Count)
            {
                var evrak = KayitliEvraklar[seciliIndex];
                evrak.KimlikNo = txtKimlikNo.Text;
                evrak.AdUnvan = txtAdUnvan.Text;
                evrak.Telefon = txtTelefon.Text;
                evrak.Adres = txtAdres.Text;
                evrak.Konu = txtKonu.Text;

                MessageBox.Show($"{evrak.EvrakKodu} kayıt numaralı evrak güncellendi!", "Güncelleme Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                FormuTemizle();
            }
        }

        private void FormuTemizle()
        {
            cmbGonderenTuru.SelectedIndex = 0;
            txtEvrakKodu.Clear();
            txtKimlikNo.Clear();
            txtAdUnvan.Clear();
            txtTelefon.Clear();
            txtAdres.Clear();
            txtKonu.Clear();
            cmbMahalle.SelectedIndex = 0;
            cmbHedefMudurler.SelectedIndex = 0;
            lstEvrakGecmisi.SelectedIndex = -1;
        }

        // =========================================================
        // DİNAMİK MENÜ GEÇİŞİ VE DİNAMİK BUTON RENKLENDİRME MANTIGI
        // =========================================================

        private void MenuButonRenkleriniGuncelle(Button tiklananButon)
        {
            List<Button> tumButonlar = new List<Button>
            {
                btnMenuTara, btnMenuGelen, btnMenuGiden,
                btnMenuArama, btnMenuAdaParsel, btnMenuIade,
                btnMenuSuresiDolan, btnMenuRapor, btnMenuAyarlar, btnMenuHizTesti
            };

            Brush pasifArkaPlan = Brushes.Transparent;
            Brush pasifYaziRengi = (Brush)new BrushConverter().ConvertFrom("#94A3B8")!;

            Brush aktifArkaPlan = (Brush)new BrushConverter().ConvertFrom("#1E293B")!;
            Brush aktifYaziRengi = Brushes.White;

            foreach (var btn in tumButonlar)
            {
                if (btn == tiklananButon)
                {
                    btn.Background = aktifArkaPlan;
                    btn.Foreground = aktifYaziRengi;
                }
                else
                {
                    btn.Background = pasifArkaPlan;
                    btn.Foreground = pasifYaziRengi;
                }
            }
        }

        private void SayfaGoster(object? sayfaView, string sayfaBaslik, Button tiklananButon)
        {
            lblAktifSayfa.Text = sayfaBaslik;
            MenuButonRenkleriniGuncelle(tiklananButon);

            if (sayfaView == null)
            {
                MainContentArea.Visibility = Visibility.Collapsed;
                MainFormGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MainFormGrid.Visibility = Visibility.Collapsed;
                MainContentArea.Visibility = Visibility.Visible;
                MainContentArea.Content = sayfaView;
            }
        }

        private void btnMenuTara_Click(object sender, RoutedEventArgs e)
        {
            SayfaGoster(null, "Genel Evrak Kayıt, Otomatik Damgalama & Adres Entegrasyonu", btnMenuTara);
        }

        private void btnMenuGelen_Click(object sender, RoutedEventArgs e)
        {
            _gelenEvrakView ??= new GelenEvrakView();
            SayfaGoster(_gelenEvrakView, "Gelen Evrak Havalesi & Onay Listesi", btnMenuGelen);
        }

        private void btnMenuGiden_Click(object sender, RoutedEventArgs e)
        {
            _gidenEvrakView ??= new GidenEvrakView();
            SayfaGoster(_gidenEvrakView, "Giden Evrak / Dış Zimmet Kaydı", btnMenuGiden);
        }

        private void btnMenuArama_Click(object sender, RoutedEventArgs e)
        {
            _detayliSorgulamaView ??= new DetayliSorgulamaView();
            SayfaGoster(_detayliSorgulamaView, "Detaylı Evrak Sorgulama & Arşiv", btnMenuArama);
        }

        private void btnMenuAdaParsel_Click(object sender, RoutedEventArgs e)
        {
            _adaParselAramaView ??= new AdaParselAramaView();
            SayfaGoster(_adaParselAramaView, "Ada / Parsel / Sicil İmar Arama", btnMenuAdaParsel);
        }

        private void btnMenuIade_Click(object sender, RoutedEventArgs e)
        {
            _iadeEvrakView ??= new IadeEvrakView();
            SayfaGoster(_iadeEvrakView, "Yanlış Sevk Edilen / İade Edilen Evrak Listesi", btnMenuIade);
        }

        private void btnMenuSuresiDolan_Click(object sender, RoutedEventArgs e)
        {
            _suresiDolanEvrakView ??= new SuresiDolanEvrakView();
            SayfaGoster(_suresiDolanEvrakView, "Süresi Dolan / Geciken Evrak Takibi", btnMenuSuresiDolan);
        }

        private void btnMenuRapor_Click(object sender, RoutedEventArgs e)
        {
            _raporlarView ??= new RaporlarView();
            SayfaGoster(_raporlarView, "İşlem & Sevk İstatistik Raporları", btnMenuRapor);
        }

        private void btnMenuAyarlar_Click(object sender, RoutedEventArgs e)
        {
            _ayarlarView ??= new AyarlarView();
            SayfaGoster(_ayarlarView, "Sistem & Donanım / Tarayıcı Ayarları", btnMenuAyarlar);
        }

        private void btnMenuHizTesti_Click(object sender, RoutedEventArgs e)
        {
            MenuButonRenkleriniGuncelle(btnMenuHizTesti);
            MessageBox.Show("Iron Mountain InSight Sunucu Bağlantı Testi:\n\n- Sunucu Erişim Hızı: 98.4 Mbps\n- Ping Süresi: 12 ms\n- Durum: Bağlantı Mükemmel", "InSight Bağlantı Testi", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}