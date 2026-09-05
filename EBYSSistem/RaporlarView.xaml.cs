using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EBYSSistem
{
    public partial class RaporlarView : UserControl
    {
        public class RaporModel
        {
            public string MudurUnvan { get; set; } = "";
            public int ToplamEvrak { get; set; }
            public int Tamamlanan { get; set; }
            public int DevamEden { get; set; }
        }

        public RaporlarView()
        {
            InitializeComponent();
        }

        private void btnRaporOlustur_Click(object sender, RoutedEventArgs e)
        {
            // GERÇEK SİSTEM İSTATİSTİĞİ: Girilen evrakların müdürlüklere göre sayısal özeti
            var mudurlukGruplari = MainWindow.KayitliEvraklar
                .GroupBy(x => x.HedefBirim)
                .Select(g => new RaporModel
                {
                    MudurUnvan = string.IsNullOrEmpty(g.Key) ? "Belirtilmedi" : g.Key,
                    ToplamEvrak = g.Count(),
                    Tamamlanan = g.Count(),
                    DevamEden = 0
                }).ToList();

            if (mudurlukGruplari.Count > 0)
            {
                dgRapor.ItemsSource = mudurlukGruplari;
                MessageBox.Show("Sistemdeki evrak kayıtlarına göre sevk raporu güncellendi.", "Rapor Hazır", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                dgRapor.ItemsSource = null;
                MessageBox.Show("Raporlanacak kayıtlı evrak bulunamadı. Lütfen önce evrak kaydı yapınız.", "Kayıt Yok", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}