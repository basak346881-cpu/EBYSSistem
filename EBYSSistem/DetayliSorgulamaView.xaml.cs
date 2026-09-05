using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EBYSSistem
{
    public partial class DetayliSorgulamaView : UserControl
    {
        public DetayliSorgulamaView()
        {
            InitializeComponent();
        }

        private void btnSorgula_Click(object sender, RoutedEventArgs e)
        {
            string tc = txtAraTC.Text.Trim();
            string ad = txtAraAd.Text.Trim().ToLower();
            string kod = txtAraKod.Text.Trim().ToLower();

            // 1. KUTULAR BOŞSA SORGU ÇALIŞMAZ
            if (string.IsNullOrEmpty(tc) && string.IsNullOrEmpty(ad) && string.IsNullOrEmpty(kod))
            {
                MessageBox.Show("Lütfen arama yapmak için T.C. Kimlik No, Ad Soyad veya Evrak Kodu alanlarından en az birini doldurunuz!", "Eksik Arama Parametresi", MessageBoxButton.OK, MessageBoxImage.Warning);
                dgSorguSonuc.ItemsSource = null;
                return;
            }

            // 2. GERÇEK KAYITLI EVRAKLAR İÇİNDE SORGULAMA YAPAR
            var aramaSonucu = MainWindow.KayitliEvraklar.Where(evrak =>
                (!string.IsNullOrEmpty(tc) && evrak.KimlikNo.Contains(tc)) ||
                (!string.IsNullOrEmpty(ad) && evrak.AdUnvan.ToLower().Contains(ad)) ||
                (!string.IsNullOrEmpty(kod) && evrak.EvrakKodu.ToLower().Contains(kod))
            ).Select(evrak => new
            {
                Tarih = evrak.Tarih,
                EvrakKodu = evrak.EvrakKodu,
                Basvuran = evrak.AdUnvan,
                Konu = evrak.Konu,
                Birim = evrak.HedefBirim,
                Durum = "Sistemde Kayıtlı / İşlemde"
            }).ToList();

            if (aramaSonucu.Count > 0)
            {
                dgSorguSonuc.ItemsSource = aramaSonucu;
            }
            else
            {
                dgSorguSonuc.ItemsSource = null;
                MessageBox.Show("Girilen kriterlere uygun kayıtlı evrak bulunamadı.", "Sonuç Yok", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // SOL KLASÖR AĞACINDAN MÜDÜRLÜK SEÇİLDİĞİNDE OTOMATİK FİLTRELEME
        private void tvKlasorler_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (tvKlasorler.SelectedItem is TreeViewItem seciliItem && seciliItem.Tag != null)
            {
                string hedefBirim = seciliItem.Tag.ToString() ?? "";

                var birimEvraklari = MainWindow.KayitliEvraklar.Where(evrak =>
                    evrak.HedefBirim.Contains(hedefBirim)
                ).Select(evrak => new
                {
                    Tarih = evrak.Tarih,
                    EvrakKodu = evrak.EvrakKodu,
                    Basvuran = evrak.AdUnvan,
                    Konu = evrak.Konu,
                    Birim = evrak.HedefBirim,
                    Durum = "Arşivde Kayıtlı"
                }).ToList();

                dgSorguSonuc.ItemsSource = birimEvraklari;

                if (birimEvraklari.Count == 0)
                {
                    MessageBox.Show($"{hedefBirim} arşiv klasöründe henüz kayıtlı evrak bulunmamaktadır.", "Klasör Boş", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}