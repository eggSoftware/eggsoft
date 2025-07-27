using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Globalization;
using System.Diagnostics; // Debug.WriteLine için gerekli

using EggSoft;

namespace EggSoft
{
    /// <summary>
    /// Interaction logic for IskontoYonetimWindow.xaml
    /// </summary>
    public partial class IskontoYonetimWindow : Window
    {
        private List<IskontoOrani> _iskontoOranlari;
        private List<Tedarikci> _tedarikciler;
        private const string ISKONTO_DATA_FILE_PATH = "iskontolar.json";
        private const string TEDARIKCI_DATA_FILE_PATH = "tedarikciler.json";

        public IskontoYonetimWindow()
        {
            InitializeComponent();
            _iskontoOranlari = new List<IskontoOrani>();
            _tedarikciler = new List<Tedarikci>();

            LoadTedarikcilerForComboBox();
            LoadYumurtaTipleriForComboBox();
            LoadIskontoOranlariFromFile();

            this.Closing += IskontoYonetimWindow_Closing;
        }

        // Tedarikçileri ComboBox'a yükleyen metod - PUBLIC VE YENİLE BUTONU İÇİN GEREKLİ
        public void LoadTedarikcilerForComboBox()
        {
            Debug.WriteLine("IskontoYonetimWindow: Tedarikçi ComboBox verileri yükleniyor...");
            if (File.Exists(TEDARIKCI_DATA_FILE_PATH))
            {
                try
                {
                    string jsonString = File.ReadAllText(TEDARIKCI_DATA_FILE_PATH);
                    _tedarikciler = JsonSerializer.Deserialize<List<Tedarikci>>(jsonString) ?? new List<Tedarikci>();
                    Debug.WriteLine($"IskontoYonetimWindow: '{TEDARIKCI_DATA_FILE_PATH}' dosyasından {_tedarikciler.Count} tedarikçi yüklendi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Tedarikçi verileri yüklenirken bir hata oluştu: {ex.Message}\nTedarikçi filtresi boş olacak.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    _tedarikciler = new List<Tedarikci>();
                    Debug.WriteLine($"IskontoYonetimWindow: Tedarikçi yükleme hatası: {ex.Message}");
                }
            }
            else
            {
                _tedarikciler = new List<Tedarikci>();
                MessageBox.Show("Tedarikçi verileri dosyası (tedarikciler.json) bulunamadı. Lütfen önce Tedarikçi Listesine tedarikçi ekleyin.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                Debug.WriteLine($"UYARI: '{TEDARIKCI_DATA_FILE_PATH}' dosyası bulunamadı.");
            }
            // ComboBox'ı "Tümü" seçeneğiyle birlikte doldur
            List<Tedarikci> tedarikciCmbListesi = new List<Tedarikci>();
            tedarikciCmbListesi.Add(new Tedarikci { Id = 0, Ad = "Tümü" }); // "Tümü" seçeneği
            tedarikciCmbListesi.AddRange(_tedarikciler.OrderBy(t => t.Ad)); // Tedarikçileri alfabetik sırala
            CmbTedarikci.ItemsSource = tedarikciCmbListesi;
            CmbTedarikci.DisplayMemberPath = "Ad";
            CmbTedarikci.SelectedValuePath = "Id";
            CmbTedarikci.SelectedIndex = 0; // Varsayılan olarak "Tümü" seçili

            Debug.WriteLine($"IskontoYonetimWindow: Tedarikçi ComboBox'ına yüklenen eleman sayısı: {tedarikciCmbListesi.Count}");
        }

        private void LoadYumurtaTipleriForComboBox()
        {
            CmbYumurtaTipi.ItemsSource = Enum.GetValues(typeof(YumurtaTipi));
        }

        private void LoadIskontoOranlariFromFile()
        {
            if (File.Exists(ISKONTO_DATA_FILE_PATH))
            {
                try
                {
                    string jsonString = File.ReadAllText(ISKONTO_DATA_FILE_PATH);
                    _iskontoOranlari = JsonSerializer.Deserialize<List<IskontoOrani>>(jsonString) ?? new List<IskontoOrani>();

                    foreach (var iskonto in _iskontoOranlari)
                    {
                        var tedarikci = _tedarikciler.FirstOrDefault(t => t.Id == iskonto.TedarikciId);
                        iskonto.TedarikciAd = tedarikci?.Ad;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"İskonto oranları yüklenirken bir hata oluştu: {ex.Message}\nBoş liste ile devam ediliyor.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    _iskontoOranlari = new List<IskontoOrani>();
                }
            }
            else
            {
                _iskontoOranlari = new List<IskontoOrani>();
            }
            IskontoDataGrid.ItemsSource = null;
            IskontoDataGrid.ItemsSource = _iskontoOranlari;
            StatusTextBlock.Text = $"{_iskontoOranlari.Count} iskonto oranı yüklendi.";
        }

        private void SaveIskontoOranlari()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_iskontoOranlari, options);
                File.WriteAllText(ISKONTO_DATA_FILE_PATH, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İskonto oranları kaydedilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IskontoYonetimWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveIskontoOranlari();
        }

        private void YeniEkle_Click(object sender, RoutedEventArgs e)
        {
            if (CmbTedarikci.SelectedItem == null || CmbYumurtaTipi.SelectedItem == null ||
                string.IsNullOrWhiteSpace(TxtHaftaNumarasi.Text) ||
                string.IsNullOrWhiteSpace(TxtOran.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtHaftaNumarasi.Text, out int haftaNumarasi) || haftaNumarasi < 1 || haftaNumarasi > 53)
            {
                MessageBox.Show("Hafta Numarası 1 ile 53 arasında geçerli bir sayı olmalıdır.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtOran.Text, out decimal oran))
            {
                MessageBox.Show("Oran alanı geçerli bir sayı olmalıdır.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int tedarikciId = (int)CmbTedarikci.SelectedValue;
            YumurtaTipi yumurtaTipi = (YumurtaTipi)CmbYumurtaTipi.SelectedItem;

            if (_iskontoOranlari.Any(io => io.TedarikciId == tedarikciId &&
                                            io.YumurtaTipi == yumurtaTipi &&
                                            io.HaftaNumarasi == haftaNumarasi))
            {
                MessageBox.Show("Bu tedarikçi, yumurta tipi ve hafta numarası için zaten bir iskonto oranı tanımlanmış.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int newId = _iskontoOranlari.Any() ? _iskontoOranlari.Max(io => io.Id) + 1 : 1;

            IskontoOrani yeniIskonto = new IskontoOrani
            {
                Id = newId,
                TedarikciId = tedarikciId,
                YumurtaTipi = yumurtaTipi,
                HaftaNumarasi = haftaNumarasi,
                Oran = oran / 100m
            };

            _iskontoOranlari.Add(yeniIskonto);
            SaveIskontoOranlari();
            LoadIskontoOranlariFromFile();
            Temizle_Click(null, null);
            StatusTextBlock.Text = "Yeni iskonto oranı başarıyla eklendi.";
        }

        private void Guncelle_Click(object sender, RoutedEventArgs e)
        {
            IskontoOrani? secilenIskonto = IskontoDataGrid.SelectedItem as IskontoOrani;

            if (secilenIskonto == null)
            {
                MessageBox.Show("Lütfen güncellemek için bir iskonto oranı seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CmbTedarikci.SelectedItem == null || CmbYumurtaTipi.SelectedItem == null ||
                string.IsNullOrWhiteSpace(TxtHaftaNumarasi.Text) ||
                string.IsNullOrWhiteSpace(TxtOran.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtHaftaNumarasi.Text, out int haftaNumarasi) || haftaNumarasi < 1 || haftaNumarasi > 53)
            {
                MessageBox.Show("Hafta Numarası 1 ile 53 arasında geçerli bir sayı olmalıdır.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtOran.Text, out decimal oran))
            {
                MessageBox.Show("Oran alanı geçerli bir sayı olmalıdır.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int tedarikciId = (int)CmbTedarikci.SelectedValue;
            YumurtaTipi yumurtaTipi = (YumurtaTipi)CmbYumurtaTipi.SelectedItem;

            if (_iskontoOranlari.Any(io => io.Id != secilenIskonto.Id &&
                                            io.TedarikciId == tedarikciId &&
                                            io.YumurtaTipi == yumurtaTipi &&
                                            io.HaftaNumarasi == haftaNumarasi))
            {
                MessageBox.Show("Bu tedarikçi, yumurta tipi ve hafta numarası için başka bir iskonto oranı zaten tanımlanmış.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            secilenIskonto.TedarikciId = tedarikciId;
            secilenIskonto.YumurtaTipi = yumurtaTipi;
            secilenIskonto.HaftaNumarasi = haftaNumarasi;
            secilenIskonto.Oran = oran / 100m;
            secilenIskonto.TedarikciAd = _tedarikciler.FirstOrDefault(t => t.Id == tedarikciId)?.Ad;

            SaveIskontoOranlari();
            LoadIskontoOranlariFromFile();
            Temizle_Click(null, null);
            StatusTextBlock.Text = "İskonto oranı başarıyla güncellendi.";
        }

        private void Sil_Click(object sender, RoutedEventArgs e)
        {
            IskontoOrani? secilenIskonto = IskontoDataGrid.SelectedItem as IskontoOrani;

            if (secilenIskonto == null)
            {
                MessageBox.Show("Lütfen silmek için bir iskonto oranı seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"'{secilenIskonto.TamAciklama}' iskonto oranını silmek istediğinizden emin misiniz?",
                "İskonto Oranı Silme Onayı",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _iskontoOranlari.Remove(secilenIskonto);
                SaveIskontoOranlari();
                LoadIskontoOranlariFromFile();
                Temizle_Click(null, null);
                StatusTextBlock.Text = "İskonto oranı başarıyla silindi.";
            }
        }

        private void Temizle_Click(object? sender, RoutedEventArgs? e)
        {
            CmbTedarikci.SelectedItem = null;
            CmbYumurtaTipi.SelectedItem = null;
            TxtHaftaNumarasi.Text = string.Empty;
            TxtOran.Text = string.Empty;
            IskontoDataGrid.SelectedItem = null;
            StatusTextBlock.Text = "Form temizlendi.";
        }

        private void IskontoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IskontoOrani? secilenIskonto = IskontoDataGrid.SelectedItem as IskontoOrani;

            if (secilenIskonto != null)
            {
                CmbTedarikci.SelectedValue = secilenIskonto.TedarikciId;
                CmbYumurtaTipi.SelectedItem = secilenIskonto.YumurtaTipi;
                TxtHaftaNumarasi.Text = secilenIskonto.HaftaNumarasi.ToString();
                TxtOran.Text = (secilenIskonto.Oran * 100m).ToString();
                StatusTextBlock.Text = $"ID: {secilenIskonto.Id} seçildi.";
            }
            else
            {
                Temizle_Click(null, null);
            }
        }

        // YENİ METOT: Tedarikçileri Yenile butonu için Click olayı
        private void TedarikcileriYenile_Click(object sender, RoutedEventArgs e)
        {
            LoadTedarikcilerForComboBox(); // Tedarikçi ComboBox'ını yeniden yükle
            LoadIskontoOranlariFromFile(); // İskonto oranlarını yeniden yükle (tedarikçi adları güncellensin diye)
            StatusTextBlock.Text = "Tedarikçi listesi ve iskonto oranları güncellendi.";
        }
    }
}