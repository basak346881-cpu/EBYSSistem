using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EBYSSistem
{
    public partial class LoginWindow : Window
    {
        private bool _kayitModu = false;

        // Belediyenin Aktif İK Personel Listesi (Sicil & TC Doğrulaması)
        private readonly Dictionary<string, string> _belediyeAktifPersonelListesi = new()
        {
            { "1001", "17627275254" }, // Başak ÖRS
            { "1002", "12345678901" }, // Ali DEMİR
            { "1003", "98765432109" }  // Ayşe KAYA
        };

        // Sisteme Kayıtlı Kullanıcılar
        private readonly Dictionary<string, (string Sifre, string Birim)> _kayitliKullanicilar
            = new(StringComparer.OrdinalIgnoreCase)
        {
            { "başak örs", ("17072005", "Bilgi İşlem Müdürlüğü") }
        };

        public LoginWindow()
        {
            InitializeComponent();
        }

        // Giriş Modunda Kullanıcı Adı Yazıldıkça Birim Otomatik Dolar
        private void txtKullaniciAdi_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_kayitModu) return;

            string kadi = txtKullaniciAdi.Text.Trim();
            if (_kayitliKullanicilar.TryGetValue(kadi, out var user))
            {
                lblBirimEtiket.Visibility = Visibility.Visible;
                cmbBirim.Visibility = Visibility.Visible;
                cmbBirim.Text = user.Birim;
                cmbBirim.IsEnabled = false; // Girişte değiştirilemez
            }
            else
            {
                lblBirimEtiket.Visibility = Visibility.Collapsed;
                cmbBirim.Visibility = Visibility.Collapsed;
            }
        }

        // Mod Değişimi (Giriş Yap <-> Kayıt Ol)
        private void btnKayitOl_Click(object sender, RoutedEventArgs e)
        {
            txtKullaniciAdi.Clear();
            txtSifre.Clear();
            txtTC.Clear();

            if (!_kayitModu)
            {
                _kayitModu = true;
                lblModBaslik.Text = "📝 Yeni Personel Kaydı & Şifre Oluşturma";

                lblTC.Visibility = Visibility.Visible;
                txtTC.Visibility = Visibility.Visible;

                lblBirimEtiket.Visibility = Visibility.Visible;
                cmbBirim.Visibility = Visibility.Visible;
                cmbBirim.IsEnabled = true; // Kayıtta birim seçimi serbest
                cmbBirim.SelectedIndex = 0;

                btnGiris.Content = "Personel Kaydımı Tamamla";
                btnKayitOl.Content = "← Giriş Ekranına Dön";
            }
            else
            {
                _kayitModu = false;
                lblModBaslik.Text = "🔑 Personel Girişi";

                lblTC.Visibility = Visibility.Collapsed;
                txtTC.Visibility = Visibility.Collapsed;

                lblBirimEtiket.Visibility = Visibility.Collapsed;
                cmbBirim.Visibility = Visibility.Collapsed;

                btnGiris.Content = "Sisteme Giriş Yap";
                btnKayitOl.Content = "👤 İlk Defa Giriş Yapıyorum (Personel Kaydı Oluştur)";
            }
        }

        private void btnGiris_Click(object sender, RoutedEventArgs e)
        {
            string kadi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Password.Trim();
            string tc = txtTC.Text.Trim();
            string birim = cmbBirim.Text;

            if (_kayitModu)
            {
                // PERSONEL KAYIT KONTROLÜ
                if (string.IsNullOrEmpty(kadi) || string.IsNullOrEmpty(tc) || string.IsNullOrEmpty(sifre))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurunuz!", "Eksik Bilgi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbBirim.SelectedIndex == 0)
                {
                    MessageBox.Show("Lütfen çalıştığınız müdürlüğü seçiniz!", "Eksik Bilgi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Belediye Çalışan Doğrulaması
                if (_belediyeAktifPersonelListesi.TryGetValue(kadi, out string? kayitliTC) && kayitliTC == tc)
                {
                    if (!_kayitliKullanicilar.ContainsKey(kadi))
                    {
                        _kayitliKullanicilar.Add(kadi, (sifre, birim));
                        MessageBox.Show("Personel kaydınız doğrulandı ve hesabınız oluşturuldu!\nŞimdi giriş yapabilirsiniz.", "Kayıt Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                        btnKayitOl_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Bu personel zaten kayıtlı! Lütfen giriş yapınız.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("⚠️ Girilen Kurum Sicil No veya T.C. Kimlik No Sultangazi Belediyesi aktif personel veritabanında bulunamadı!\nDışarıdan yetkisiz kişilerin sisteme kaydolması engellenmiştir.", "Kayıt Reddedildi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // GİRİŞ KONTROLÜ
                if (_kayitliKullanicilar.TryGetValue(kadi, out var user) && user.Sifre == sifre)
                {
                    MainWindow main = new MainWindow(kadi.ToUpper(), user.Birim);
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hatalı Kullanıcı Adı veya Şifre!", "Giriş Engellendi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}