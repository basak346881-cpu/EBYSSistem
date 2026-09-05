using System.Windows;

namespace EBYSSistem
{
    public partial class DocumentViewerWindow : Window
    {
        public DocumentViewerWindow(string evrakKodu, string tcNo, string adSoyad, string konu, string mahalle, string adres, string tel, string tarih, string hedefBirim)
        {
            InitializeComponent();

            lblEvrakBaslik.Text = $"EVRAK KODU: {evrakKodu}";
            lblEvrakTarih.Text = $"Kayıt Tarihi: {tarih}";

            lblDamgaEvrakNo.Text = $"EVRAK NO: {evrakKodu}";
            lblDamgaTarih.Text = $"TARİH   : {tarih}";

            lblOnizleKonu.Text = $"KONU: {konu}";
            lblOnizleKisi.Text = $"Başvuran Vatandaş: {adSoyad}\n(T.C. Kimling No: {tcNo})\nSevk Yapılan Birim: {hedefBirim}\n\nSultangazi Belediye Başkanlığına sunulmuş olan yukarıdaki başvuru evrakı sisteme taranarak kaydolunmuştur ve ilgili birime havale edilmiştir.";
            lblOnizleAdres.Text = $"İkamet Adresi: {mahalle}, {adres} Sultangazi / İSTANBUL";
            lblOnizleTel.Text = $"İletişim Tel: {tel}";
        }
    }
}