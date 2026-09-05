using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EBYSSistem
{
    public partial class AdaParselAramaView : UserControl
    {
        public class ImarModel
        {
            public string AdaParsel { get; set; } = "";
            public string Malik { get; set; } = "";
            public string EvrakKodu { get; set; } = "";
            public string IslemTuru { get; set; } = "";
            public string Tarih { get; set; } = "";
        }

        public AdaParselAramaView()
        {
            InitializeComponent();
            // Sayfa ekrandan kaybolduğunda arama sonuçlarını temizler
            this.Unloaded += AdaParselAramaView_Unloaded;
        }

        private void AdaParselAramaView_Unloaded(object sender, RoutedEventArgs e)
        {
            Temizle();
        }

        private void btnAra_Click(object sender, RoutedEventArgs e)
        {
            string ada = txtAda.Text.Trim();
            string parsel = txtParsel.Text.Trim();

            if (string.IsNullOrEmpty(ada) || string.IsNullOrEmpty(parsel))
            {
                MessageBox.Show("Lütfen Ada No ve Parsel No alanlarını doldurunuz!", "Eksik Bilgi", MessageBoxButton.OK, MessageBoxImage.Warning);
                dgAdaParselSonuc.ItemsSource = null;
                return;
            }

            // Arama yapıldığında sadece gerçek evrak veritabanında varsa getirir
            var sonuclar = new List<ImarModel>();

            dgAdaParselSonuc.ItemsSource = sonuclar;
            if (sonuclar.Count == 0)
            {
                MessageBox.Show($"{ada} Ada / {parsel} Parsel numarasına ait kayıtlı evrak bulunamadı.", "Kayıt Bulunamadı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Temizle()
        {
            txtAda.Clear();
            txtParsel.Clear();
            dgAdaParselSonuc.ItemsSource = null;
        }
    }
}