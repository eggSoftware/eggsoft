using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http; // Hala kalsın, ileride kullanacağız
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Diagnostics;

using HtmlAgilityPack; // Hala kalsın, ileride kullanacağız

using EggSoft;

namespace EggSoft
{
    public partial class BasmakciFiyatYonetimWindow : Window
    {
        private List<BasmakciFiyat> _basmakciFiyatlari;
        private const string DATA_FILE_PATH = "basmakciFiyatlari.json";
        private HttpClient _httpClient;

        public BasmakciFiyatYonetimWindow()
        {
            InitializeComponent();
            _basmakciFiyatlari = new List<BasmakciFiyat>();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36");

            LoadBasmakciFiyatlariFromFile();
            this.Closing += BasmakciFiyatYonetimWindow_Closing;
        }

        private void LoadBasmakciFiyatlariFromFile()
        {
            if (File.Exists(DATA_FILE_PATH))
            {
                try
                {
                    string jsonString = File.ReadAllText(DATA_FILE_PATH);
                    _basmakciFiyatlari = JsonSerializer.Deserialize<List<BasmakciFiyat>>(jsonString) ?? new List<BasmakciFiyat>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Başmakçı fiyatları yüklenirken bir hata oluştu: {ex.Message}\\nBoş liste ile devam ediliyor.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    _basmakciFiyatlari = new List<BasmakciFiyat>();
                }
            }
            else
            {
                _basmakciFiyatlari = new List<BasmakciFiyat>();
            }
            BasmakciFiyatDataGrid.ItemsSource = null;
            BasmakciFiyatDataGrid.ItemsSource = _basmakciFiyatlari;
            UpdateStatusText();
        }

        private void SaveBasmakciFiyatlari()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_basmakciFiyatlari, options);
                File.WriteAllText(DATA_FILE_PATH, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Başmakçı fiyatları kaydedilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BasmakciFiyatYonetimWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveBasmakciFiyatlari();
        }

        private void UpdateStatusText(string message = "")
        {
            if (!string.IsNullOrEmpty(message))
            {
                StatusTextBlock.Text = message;
                TxtSonCekimDurumu.Text = message;
            }
            else if (_basmakciFiyatlari != null && _basmakciFiyatlari.Any())
            {
                BasmakciFiyat? sonFiyat = _basmakciFiyatlari.OrderByDescending(f => f.Tarih).FirstOrDefault();
                if (sonFiyat != null)
                {
                    TxtSonCekimDurumu.Text = $"Son çekim: {sonFiyat.Tarih.ToShortDateString()} - {sonFiyat.DubleYumurtaFiyati:C2} TL (Hafta {sonFiyat.HaftaNumarasi})";
                    StatusTextBlock.Text = $"Toplam {_basmakciFiyatlari.Count} fiyat kaydı yüklendi.";
                }
                else
                {
                    TxtSonCekimDurumu.Text = "Henüz fiyat çekilmedi.";
                    StatusTextBlock.Text = "Henüz fiyat kaydı bulunmuyor.";
                }
            }
            else
            {
                TxtSonCekimDurumu.Text = "Henüz fiyat çekilmedi.";
                StatusTextBlock.Text = "Henüz fiyat kaydı bulunmuyor.";
            }
        }

        // Fiyat Çek butonu (Şimdilik web scraping atlandı, simülasyon yapılıyor)
        private async void FiyatCek_Click(object sender, RoutedEventArgs e)
        {
            StatusTextBlock.Text = "Fiyat simüle ediliyor...";
            TxtSonCekimDurumu.Text = "Fiyat simüle ediliyor...";

            try
            {
                // Hafta numarası ve tarih belirleme
                DateTime today = DateTime.Today;
                System.Globalization.Calendar cal = CultureInfo.CurrentCulture.Calendar;
                // Bu kural, bir yılın ilk haftasını, içinde en az 4 gün bulunan ilk hafta olarak tanımlar.
                int currentWeek = cal.GetWeekOfYear(today, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                // Aynı haftadan zaten fiyat varsa tekrar ekleme
                if (_basmakciFiyatlari.Any(f => f.HaftaNumarasi == currentWeek && f.Tarih.Year == today.Year))
                {
                    MessageBox.Show("Bu hafta için fiyat zaten eklenmiş.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateStatusText("Bu hafta için fiyat zaten eklenmiş.");
                    LoadBasmakciFiyatlariFromFile(); // DataGrid'i yenile
                    return;
                }

                // SIMÜLE EDİLMİŞ FİYAT: Sabit bir değer ekliyoruz
                decimal cekilenFiyat = 4.00m; // Örnek olarak 4.00 TL belirliyoruz

                // Gerekirse farklı fiyatlar simüle edebilirsiniz
                // Random rnd = new Random();
                // cekilenFiyat = (decimal)(rnd.NextDouble() * 1.0 + 3.5); // 3.5 ile 4.5 arası rastgele fiyat

                // Yeni fiyat kaydını oluştur
                BasmakciFiyat yeniFiyat = new BasmakciFiyat
                {
                    Id = _basmakciFiyatlari.Any() ? _basmakciFiyatlari.Max(f => f.Id) + 1 : 1,
                    Tarih = today,
                    DubleYumurtaFiyati = cekilenFiyat,
                    HaftaNumarasi = currentWeek
                };

                _basmakciFiyatlari.Add(yeniFiyat);
                SaveBasmakciFiyatlari(); // Yeni fiyatı kaydet
                LoadBasmakciFiyatlariFromFile(); // DataGrid'i ve durumu yenile
                MessageBox.Show($"Başmakçı fiyatı başarıyla simüle edildi: {cekilenFiyat:C2} TL", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                StatusTextBlock.Text = "Fiyat simülasyonu tamamlandı.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fiyat simüle edilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusTextBlock.Text = "Fiyat simülasyonunda hata oluştu.";
            }
            // Web scraping kodları şimdilik yoruma alındı/kaldırıldı.
            // Bu kısma ileride geri döneceğiz.
        }
    }
}