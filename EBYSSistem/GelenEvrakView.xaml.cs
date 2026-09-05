using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EBYSSistem
{
    public partial class GelenEvrakView : UserControl
    {
        public class GelenModel
        {
            public string EvrakKodu { get; set; } = "";
            public string GelenMakam { get; set; } = "";
            public string Konu { get; set; } = "";
            public string Tarih { get; set; } = "";
            public string HedefBirim { get; set; } = "";
        }

        public GelenEvrakView()
        {
            InitializeComponent();
        }

        private void btnYenile_Click(object sender, RoutedEventArgs e)
        {
            // GERÇEK SİSTEM TEMİZLİĞİ: Sahte kayıtlar kaldırıldı.
            List<GelenModel> gelenBekleyenler = new List<GelenModel>();

            dgGelenEvraklar.ItemsSource = gelenBekleyenler;
            dgGuncellemeBekleyenler.ItemsSource = gelenBekleyenler;
            dgImzaBekleyenler.ItemsSource = gelenBekleyenler;

            MessageBox.Show("Sisteme henüz dış makamlardan veya birimlerden düşen havale/onay bekleyen evrak bulunmamaktadır.", "Gelen Evrak Durumu", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void tabGelenEvrakSürecleri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Sekmeler arası geçiş yapıldığında gerekirse listeleri güncel tutar
        }
    }
}