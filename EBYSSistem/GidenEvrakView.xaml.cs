using System.Windows;
using System.Windows.Controls;

namespace EBYSSistem
{
    public partial class GidenEvrakView : UserControl
    {
        public GidenEvrakView()
        {
            InitializeComponent();
        }

        private void btnKaydetYazdir_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtKurum.Text) || string.IsNullOrEmpty(txtKonu.Text))
            {
                MessageBox.Show("Lütfen Gideceği Kurum ve Konu alanlarını doldurunuz!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string zimmet = $"{txtKurum.Text} | Zimmet No: {txtZimmetNo.Text} | Konu: {txtKonu.Text}";
            lstZimmetGecmisi.Items.Insert(0, zimmet);

            MessageBox.Show("Dış zimmet kaydı yapıldı ve kurye zimmet teslim çıktısı yazıcıya gönderildi.", "İşlem Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);

            txtKurum.Clear();
            txtZimmetNo.Clear();
            txtKonu.Clear();
        }
    }
}