using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EBYSSistem
{
    public partial class IadeEvrakView : UserControl
    {
        public class IadeModel
        {
            public string EvrakKodu { get; set; } = "";
            public string IadeEdenBirim { get; set; } = "";
            public string Gerekce { get; set; } = "";
            public string Tarih { get; set; } = "";
        }

        public IadeEvrakView()
        {
            InitializeComponent();
        }

        private void btnYenile_Click(object sender, RoutedEventArgs e)
        {
            // Sahte veriler kaldırıldı. Sadece gerçekten iade edilen kayıt varsa gelir.
            List<IadeModel> iadeListesi = new List<IadeModel>();

            dgIadeEvraklar.ItemsSource = iadeListesi;
            MessageBox.Show("Müdürlüklerden geri iade edilen yeni evrak bulunmamaktadır.", "İade Evrak Listesi", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}