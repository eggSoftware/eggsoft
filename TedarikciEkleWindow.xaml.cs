using System;       // Random sınıfı için gerekli
using System.Windows;   // Window, MessageBox, RoutedEventArgs için gerekli

// Tedarikci sınıfına erişmek için EggSoft namespace'ini kullanıyoruz
using EggSoft;

namespace EggSoft
{
    /// <summary>
    /// Interaction logic for TedarikciEkleWindow.xaml
    /// </summary>
    public partial class TedarikciEkleWindow : Window
    {
        // Bu property, yeni eklenen tedarikçi nesnesini ana pencereye geri döndürmek için kullanılacak.
        // "?" işareti, null atanabilir olduğunu gösterir (hiçbir şey eklenmezse null kalabilir).
        public Tedarikci? YeniTedarikci { get; private set; }

        public TedarikciEkleWindow()
        {
            InitializeComponent();
        }

        // "Kaydet" butonuna tıklanınca çalışacak metod
        private void Kaydet_Click(object sender, RoutedEventArgs e)
        {
            // Alanların boş olup olmadığını kontrol et
            if (string.IsNullOrWhiteSpace(TxtAd.Text) ||
                string.IsNullOrWhiteSpace(TxtSehir.Text) ||
                string.IsNullOrWhiteSpace(TxtTelefon.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Alanlar boşsa işlemi durdur
            }

            // Yeni bir Tedarikci nesnesi oluştur
            YeniTedarikci = new Tedarikci
            {
                // Basit bir ID ataması yapalım, ileride daha güvenli hale getirilebilir.
                // Şimdilik 1000 ile 9999 arasında rastgele bir sayı atıyoruz.
                Id = new Random().Next(1000, 9999),
                Ad = TxtAd.Text,
                Sehir = TxtSehir.Text,
                Telefon = TxtTelefon.Text
            };

            // Pencereyi kapatırken, işlemin başarılı olduğunu belirtir.
            // Bu sayede TedarikciListesiWindow, bu pencereden gelen bilgiyi alabilir.
            DialogResult = true;
        }
    }
}