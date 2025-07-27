using System.Windows;   // Window, RoutedEventArgs için gerekli

// Tedarikci sınıfına erişmek için EggSoft namespace'ini kullanıyoruz
using EggSoft;

namespace EggSoft
{
    /// <summary>
    /// Interaction logic for TedarikciDuzenleWindow.xaml
    /// </summary>
    public partial class TedarikciDuzenleWindow : Window
    {
        // Düzenlenecek Tedarikçi nesnesini tutar. Bu nesne dışarıdan (TedarikciListesiWindow'dan) verilecek.
        public Tedarikci TedarikciToEdit { get; private set; }

        // Yapıcı metod: Pencere açılırken düzenlenecek tedarikçi nesnesi alınır.
        public TedarikciDuzenleWindow(Tedarikci tedarikci)
        {
            InitializeComponent();
            TedarikciToEdit = tedarikci; // Gelen tedarikçi nesnesini sakla

            // Tedarikçi bilgilerini form alanlarına yükle
            TxtId.Text = TedarikciToEdit.Id.ToString();
            TxtAd.Text = TedarikciToEdit.Ad;
            TxtSehir.Text = TedarikciToEdit.Sehir;
            TxtTelefon.Text = TedarikciToEdit.Telefon;
        }

        // "Kaydet" butonuna tıklanınca çalışacak metod
        private void Kaydet_Click(object sender, RoutedEventArgs e)
        {
            // Alanların boş olup olmadığını kontrol et (isteğe bağlı, ama iyi bir pratik)
            if (string.IsNullOrWhiteSpace(TxtAd.Text) ||
                string.IsNullOrWhiteSpace(TxtSehir.Text) ||
                string.IsNullOrWhiteSpace(TxtTelefon.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Alanlar boşsa işlemi durdur
            }

            // Formdaki güncel bilgileri TedarikciToEdit nesnesine yaz
            TedarikciToEdit.Ad = TxtAd.Text;
            TedarikciToEdit.Sehir = TxtSehir.Text;
            TedarikciToEdit.Telefon = TxtTelefon.Text;

            // Pencereyi kapatırken, işlemin başarılı olduğunu belirtir.
            // Bu sayede TedarikciListesiWindow, bu pencereden gelen bilgiyi alıp listeyi güncelleyebilir.
            DialogResult = true;
        }
    }
}