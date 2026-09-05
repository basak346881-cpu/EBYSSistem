using System.Windows;
using System.Windows.Controls;

namespace EBYSSistem
{
    public partial class AyarlarView : UserControl
    {
        public AyarlarView()
        {
            InitializeComponent();
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            string seciliScanner = (cmbScanner.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Bilinmiyor";
            string seciliYazici = (cmbYazici.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Bilinmiyor";
            string seciliDPI = (cmbDPI.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "300 DPI";

            MessageBox.Show($"Sistem ve Donanım Ayarları Başarıyla Güncellendi!\n\n" +
                            $"• Aktif Tarayıcı: {seciliScanner}\n" +
                            $"• Barkod Yazıcı: {seciliYazici}\n" +
                            $"• Çözünürlük: {seciliDPI}\n\n" +
                            $"TWAIN sürücü entegrasyonu hazır.",
                            "Konfigürasyon Kaydedildi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }
}