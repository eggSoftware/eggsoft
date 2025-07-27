using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

// EggSoft namespace'ini kullanarak Tedarikci sınıfına erişiyoruz
using EggSoft;

namespace EggSoft
{
    /// <summary>
    /// Interaction logic for TedarikciListesiWindow.xaml
    /// </summary>
    public partial class TedarikciListesiWindow : Window
    {
        // Tedarikçileri tutan liste
        private List<Tedarikci> _tedarikciler;
        // Verilerin kaydedileceği dosya yolu
        private const string DATA_FILE_PATH = "tedarikciler.json";

        public TedarikciListesiWindow()
        {
            InitializeComponent();
            _tedarikciler = new List<Tedarikci>(); // Listeyi başlat

            // Tedarikçileri dosyadan yükle
            LoadTedarikcilerFromFile();

            // Pencere kapanırken verileri kaydetme olayını ekle
            this.Closing += TedarikciListesiWindow_Closing;
        }

        // Tedarikçileri JSON dosyasından yükleyen metod
        private void LoadTedarikcilerFromFile()
        {
            if (File.Exists(DATA_FILE_PATH))
            {
                try
                {
                    string jsonString = File.ReadAllText(DATA_FILE_PATH);
                    // JSON'dan Tedarikci listesine dönüştür, eğer null gelirse yeni liste oluştur.
                    _tedarikciler = JsonSerializer.Deserialize<List<Tedarikci>>(jsonString) ?? new List<Tedarikci>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Veriler yüklenirken bir hata oluştu: {ex.Message}\nBoş liste ile devam ediliyor.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    _tedarikciler = new List<Tedarikci>(); // Hata durumunda boş liste ile devam et
                }
            }
            else
            {
                // Dosya yoksa, başlangıç için örnek tedarikçiler ekle
                _tedarikciler = new List<Tedarikci>
                {
                    new Tedarikci { Id = 1, Ad = "ABC Yumurta", Sehir = "Afyon", Telefon = "0272 111 2233" },
                    new Tedarikci { Id = 2, Ad = "Doğa Çiftlik", Sehir = "Uşak", Telefon = "0276 444 5566" },
                    new Tedarikci { Id = 3, Ad = "Bereket Gıda", Sehir = "Denizli", Telefon = "0258 777 8899" },
                    new Tedarikci { Id = 4, Ad = "Can Tavukçuluk", Sehir = "Konya", Telefon = "0332 123 4567" }
                };
                SaveTedarikciler(); // Örnek verileri dosyaya kaydet
            }
            // DataGrid'i güncellemek için ItemsSource'u null yapıp tekrar ata
            TedarikciDataGrid.ItemsSource = null;
            TedarikciDataGrid.ItemsSource = _tedarikciler;
        }

        // Tedarikçileri JSON dosyasına kaydeden metod
        private void SaveTedarikciler()
        {
            try
            {
                // JSON'u daha okunabilir formatta kaydetmek için seçenekler
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_tedarikciler, options);
                File.WriteAllText(DATA_FILE_PATH, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler kaydedilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Pencere kapanırken otomatik kaydetme
        private void TedarikciListesiWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveTedarikciler();
        }

        // DataGrid'de seçim değiştiğinde detayları gösteren metod
        private void TedarikciDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tedarikci? secilenTedarikci = TedarikciDataGrid.SelectedItem as Tedarikci; // Null atanabilir olarak işaretlendi

            if (secilenTedarikci != null)
            {
                // Seçilen tedarikçinin detaylarını TextBlock'lara yaz
                TxtDetayId.Text = secilenTedarikci.Id.ToString();
                TxtDetayAd.Text = secilenTedarikci.Ad;
                TxtDetaySehir.Text = secilenTedarikci.Sehir;
                TxtDetayTelefon.Text = secilenTedarikci.Telefon;
            }
            else
            {
                // Seçim kaldırılırsa detayları temizle
                TxtDetayId.Text = string.Empty;
                TxtDetayAd.Text = string.Empty;
                TxtDetaySehir.Text = string.Empty;
                TxtDetayTelefon.Text = string.Empty;
            }
        }

        // Menüdeki Çıkış butonuna tıklanınca pencereyi kapatır
        private void MenuCikis_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Yeni Tedarikçi Ekle butonu
        private void MenuYeniTedarikciEkle_Click(object sender, RoutedEventArgs e)
        {
            TedarikciEkleWindow ekleWindow = new TedarikciEkleWindow();

            // Pencereyi bir diyalog olarak aç (yani bu pencere kapanana kadar arka plandaki pencereye erişilemez)
            if (ekleWindow.ShowDialog() == true) // Kullanıcı "Kaydet" butonuna bastıysa ve DialogResult true ise
            {
                if (ekleWindow.YeniTedarikci != null) // Yeni tedarikçi nesnesi oluşturulduysa
                {
                    _tedarikciler.Add(ekleWindow.YeniTedarikci); // Listeye ekle
                    SaveTedarikciler(); // JSON dosyasına kaydet
                    LoadTedarikcilerFromFile(); // DataGrid'i yenile (dosyadan yeniden yükleyerek)
                    MessageBox.Show("Yeni tedarikçi başarıyla eklendi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Tedarikçiyi Düzenle butonu
        private void MenuTedarikciDuzenle_Click(object sender, RoutedEventArgs e)
        {
            Tedarikci? secilenTedarikci = TedarikciDataGrid.SelectedItem as Tedarikci;

            if (secilenTedarikci == null)
            {
                MessageBox.Show("Lütfen düzenlemek için bir tedarikçi seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Seçim yapılmadıysa işlemi durdur
            }

            // Seçilen tedarikçi nesnesini düzenleme penceresine gönder
            TedarikciDuzenleWindow duzenleWindow = new TedarikciDuzenleWindow(secilenTedarikci);

            if (duzenleWindow.ShowDialog() == true) // Kullanıcı "Kaydet" butonuna bastıysa
            {
                // Tedarikci nesnesi zaten referans olarak güncellendiği için sadece kaydet ve yenile yeterli
                SaveTedarikciler(); // JSON dosyasına güncellenmiş veriyi kaydet
                LoadTedarikcilerFromFile(); // DataGrid'i yenile
                MessageBox.Show("Tedarikçi bilgileri başarıyla güncellendi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Tedarikçiyi Sil butonu
        private void MenuTedarikciSil_Click(object sender, RoutedEventArgs e)
        {
            Tedarikci? secilenTedarikci = TedarikciDataGrid.SelectedItem as Tedarikci;

            if (secilenTedarikci == null)
            {
                MessageBox.Show("Lütfen silmek için bir tedarikçi seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Seçim yapılmadıysa işlemi durdur
            }

            // Kullanıcıdan onay al
            MessageBoxResult result = MessageBox.Show(
                $"'{secilenTedarikci.Ad}' adlı tedarikçiyi silmek istediğinizden emin misiniz?",
                "Tedarikçi Silme Onayı",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _tedarikciler.Remove(secilenTedarikci); // Listeden sil
                SaveTedarikciler(); // JSON dosyasına kaydet
                LoadTedarikcilerFromFile(); // DataGrid'i yenile
                MessageBox.Show("Tedarikçi başarıyla silindi!", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}