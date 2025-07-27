using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Globalization;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Reflection;

using EggSoft;

namespace EggSoft
{
    public partial class YumurtaAlimWindow : Window
    {
        private List<YumurtaAlim> _yumurtaAlimlari;
        private List<Tedarikci> _tumTedarikciler;
        private const string DATA_FILE_PATH = "yumurtaAlimlari.json";
        private const string TEDARIKCILER_FILE_PATH = "tedarikciler.json";
        private string _selectedFilePath = string.Empty;

        public YumurtaAlimWindow()
        {
            InitializeComponent();
            _yumurtaAlimlari = new List<YumurtaAlim>();
            _tumTedarikciler = new List<Tedarikci>();

            LoadYumurtaAlimlariFromFile();
            LoadTedarikcilerFromFile();

            this.Closing += YumurtaAlimWindow_Closing;
        }

        private void LoadTedarikcilerFromFile()
        {
            Debug.WriteLine($"YumurtaAlimWindow: Tedarikçi yükleme başlatıldı.");
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TEDARIKCILER_FILE_PATH);
            Debug.WriteLine($"YumurtaAlimWindow: Tedarikçiler dosyası aranıyor: {fullPath}");

            if (File.Exists(fullPath))
            {
                Debug.WriteLine($"YumurtaAlimWindow: '{fullPath}' dosyası bulundu.");
                try
                {
                    string jsonString = File.ReadAllText(fullPath);
                    Debug.WriteLine($"YumurtaAlimWindow: Tedarikçi dosyasından okunan JSON içeriği boyutu: {jsonString.Length} karakter.");
                    _tumTedarikciler = JsonSerializer.Deserialize<List<Tedarikci>>(jsonString) ?? new List<Tedarikci>();
                    Debug.WriteLine($"YumurtaAlimWindow: JSON'dan deserialize edilen tedarikçi sayısı: {_tumTedarikciler.Count}");
                    if (!_tumTedarikciler.Any())
                    {
                        Debug.WriteLine("YumurtaAlimWindow: Deserialization sonrası tedarikçi listesi boş.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Tedarikçi verileri yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    _tumTedarikciler = new List<Tedarikci>();
                    Debug.WriteLine($"YumurtaAlimWindow: Tedarikçi yükleme hatası: {ex.Message}");
                }
            }
            else
            {
                _tumTedarikciler = new List<Tedarikci>();
                MessageBox.Show($"Tedarikçi verileri dosyası ({TEDARIKCILER_FILE_PATH} - Tam yol: {fullPath}) bulunamadı. Lütfen önce Tedarikçi Listesine tedarikçi ekleyin.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                Debug.WriteLine($"YumurtaAlimWindow: UYARI: '{fullPath}' dosyası bulunamadı.");
            }
        }

        private int GetNextTedarikciId()
        {
            return _tumTedarikciler.Any() ? _tumTedarikciler.Max(t => t.Id) + 1 : 1;
        }

        private void LoadYumurtaAlimlariFromFile()
        {
            if (File.Exists(DATA_FILE_PATH))
            {
                try
                {
                    string jsonString = File.ReadAllText(DATA_FILE_PATH);
                    _yumurtaAlimlari = JsonSerializer.Deserialize<List<YumurtaAlim>>(jsonString) ?? new List<YumurtaAlim>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Yumurta alım verileri yüklenirken bir hata oluştu: {ex.Message}\nBoş liste ile devam ediliyor.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    _yumurtaAlimlari = new List<YumurtaAlim>();
                }
            }
            else
            {
                _yumurtaAlimlari = new List<YumurtaAlim>();
            }
            YumurtaAlimDataGrid.ItemsSource = null;
            YumurtaAlimDataGrid.ItemsSource = _yumurtaAlimlari;
            UpdateStatusText($"{_yumurtaAlimlari.Count} alım kaydı yüklendi.");
        }

        private void SaveYumurtaAlimlari()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_yumurtaAlimlari, options);
                File.WriteAllText(DATA_FILE_PATH, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yumurta alım verileri kaydedilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveTedarikciler()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_tumTedarikciler, options);
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TEDARIKCILER_FILE_PATH);
                File.WriteAllText(fullPath, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Tedarikçi verileri kaydedilirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void YumurtaAlimWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveYumurtaAlimlari();
        }

        private void UpdateStatusText(string message)
        {
            StatusTextBlock.Text = message;
        }

        private void DosyaSec_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Metin ve CSV Dosyaları (*.txt;*.csv;*.tsv)|*.txt;*.csv;*.tsv|Tüm Dosyalar (*.*)|*.*";
            openFileDialog.Title = "Yumurta Alım Verisi Dosyası Seçin";

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = openFileDialog.FileName;
                TxtFilePath.Text = _selectedFilePath;
                UpdateStatusText($"'{Path.GetFileName(_selectedFilePath)}' dosyası seçildi. Yüklemek için 'Verileri Yükle / Kaydet'e tıklayın.");
            }
            else
            {
                _selectedFilePath = string.Empty;
                TxtFilePath.Text = string.Empty;
                UpdateStatusText("Dosya seçimi iptal edildi.");
            }
        }

        private void VerileriYukleKaydet_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath) || !File.Exists(_selectedFilePath))
            {
                MessageBox.Show("Lütfen önce yüklenecek bir dosya seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            char delimiter = '\0';
            string[] lines;
            try
            {
                lines = File.ReadAllLines(_selectedFilePath);
                if (lines.Length <= 1)
                {
                    MessageBox.Show("Seçilen dosyada başlık satırı dışında okunacak veri bulunamadı veya dosya boş.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    UpdateStatusText("Dosyada geçerli veri bulunamadı.");
                    return;
                }

                string firstDataLine = lines[0];

                if (firstDataLine.Contains('\t'))
                {
                    delimiter = '\t';
                }
                else if (firstDataLine.Contains(';'))
                {
                    delimiter = ';';
                }
                else if (firstDataLine.Contains(','))
                {
                    delimiter = ',';
                }
                else
                {
                    MessageBox.Show("Dosya içeriğinde geçerli bir sütun ayırıcısı (tab, noktalı virgül veya virgül) bulunamadı. Lütfen dosya formatını kontrol edin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpdateStatusText("Geçerli ayırıcı bulunamadı.");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Dosya okunurken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                UpdateStatusText("Dosya okuma hatası.");
                return;
            }

            List<YumurtaAlim> yeniAlimlar = new List<YumurtaAlim>();
            int basariliIslemSayisi = 0;
            int hataSayisi = 0;
            string detayliHataMesaji = "";
            bool yeniTedarikciEklendi = false;

            const int TarihIndex = 0;
            const int NetKgIndex = 5;
            const int YumurtaGramajiIndex = 6;
            const int YumurtaAdetIndex = 7;
            const int EtiketNoIndex = 8;
            const int CariAdIndex = 10;
            const int StokAdIndex = 11;

            const int EXPECTED_COLUMN_COUNT = 12;

            for (int i = 1; i < lines.Length; i++)
            {
                string[] cells = lines[i].Split(new char[] { delimiter }, StringSplitOptions.None);

                Debug.WriteLine($"Satır {i + 1} orijinal: '{lines[i]}'");
                Debug.WriteLine($"Satır {i + 1} kullanılan ayırıcı: '{delimiter}'");
                Debug.WriteLine($"Satır {i + 1} ayrılmış hücre sayısı: {cells.Length}");
                for (int k = 0; k < cells.Length; k++)
                {
                    Debug.WriteLine($"  cells[{k}]: '{cells[k]}'");
                }

                if (cells.Length < EXPECTED_COLUMN_COUNT)
                {
                    string msg = $"Satır {i + 1} atlandı: Yetersiz sütun sayısı. Beklenen minimum: {EXPECTED_COLUMN_COUNT}, Bulunan: {cells.Length}. Satır içeriği: '{lines[i]}'";
                    UpdateStatusText(msg);
                    detayliHataMesaji += msg + Environment.NewLine;
                    hataSayisi++;
                    continue;
                }

                try
                {
                    DateTime tarih;
                    string tarihString = cells[TarihIndex].Trim();
                    if (double.TryParse(tarihString, CultureInfo.InvariantCulture, out double oaDate))
                    {
                        tarih = DateTime.FromOADate(oaDate);
                    }
                    else if (!DateTime.TryParseExact(tarihString, "d.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out tarih) &&
                             !DateTime.TryParseExact(tarihString, "d.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tarih))
                    {
                        throw new FormatException($"Tarih formatı geçersiz: '{tarihString}'");
                    }

                    string netKgStr = cells.Length > NetKgIndex ? cells[NetKgIndex].Trim().Replace(",", ".") : throw new IndexOutOfRangeException($"NetKg sütunu bulunamadı (index {NetKgIndex})");
                    decimal netKg = decimal.Parse(netKgStr, CultureInfo.InvariantCulture);

                    string yumurtaGramajiStr = cells.Length > YumurtaGramajiIndex ? cells[YumurtaGramajiIndex].Trim().Replace(",", ".") : throw new IndexOutOfRangeException($"YumurtaGramaji sütunu bulunamadı (index {YumurtaGramajiIndex})");
                    decimal yumurtaGramaji = decimal.Parse(yumurtaGramajiStr, CultureInfo.InvariantCulture);

                    string yumurtaAdediStr = cells.Length > YumurtaAdetIndex ? cells[YumurtaAdetIndex].Trim() : throw new IndexOutOfRangeException($"YumurtaAdedi sütunu bulunamadı (index {YumurtaAdetIndex})");
                    int yumurtaAdedi = int.Parse(yumurtaAdediStr, CultureInfo.InvariantCulture);

                    string etiketNo = cells.Length > EtiketNoIndex ? cells[EtiketNoIndex].Trim() : throw new IndexOutOfRangeException($"EtiketNo sütunu bulunamadı (index {EtiketNoIndex})");
                    string cariAd = cells.Length > CariAdIndex ? cells[CariAdIndex].Trim() : throw new IndexOutOfRangeException($"CariAd sütunu bulunamadı (index {CariAdIndex})");
                    string stokAd = cells.Length > StokAdIndex ? cells[StokAdIndex].Trim() : throw new IndexOutOfRangeException($"StokAd sütunu bulunamadı (index {StokAdIndex})");

                    Tedarikci? mevcutTedarikci = _tumTedarikciler.FirstOrDefault(t => t.Ad.Equals(cariAd, StringComparison.OrdinalIgnoreCase));

                    if (mevcutTedarikci == null)
                    {
                        Tedarikci yeniTedarikci = new Tedarikci
                        {
                            Id = GetNextTedarikciId(),
                            Ad = cariAd,
                            Sehir = "Bilinmiyor",
                            Telefon = "Bilinmiyor"
                        };
                        _tumTedarikciler.Add(yeniTedarikci);
                        yeniTedarikciEklendi = true;
                        Debug.WriteLine($"Yeni tedarikçi eklendi: {yeniTedarikci.Ad} (ID: {yeniTedarikci.Id})");
                    }

                    if (_yumurtaAlimlari.Any(a => a.EtiketNo == etiketNo && a.Tarih.Date == tarih.Date))
                    {
                        string msg = $"Satır {i + 1} atlandı: Bu etiket numarası ve tarih için zaten bir alım mevcut ({etiketNo} - {tarih.ToShortDateString()}).";
                        UpdateStatusText(msg);
                        detayliHataMesaji += msg + Environment.NewLine;
                        hataSayisi++;
                        continue;
                    }

                    YumurtaAlim yeniAlim = new YumurtaAlim
                    {
                        Id = _yumurtaAlimlari.Any() ? _yumurtaAlimlari.Max(a => a.Id) + 1 : 1,
                        Tarih = tarih,
                        NetKg = netKg,
                        YumurtaGramaji = yumurtaGramaji,
                        YumurtaAdedi = yumurtaAdedi,
                        EtiketNo = etiketNo,
                        CariAd = cariAd,
                        StokAd = stokAd
                    };
                    yeniAlimlar.Add(yeniAlim);
                    basariliIslemSayisi++;
                }
                catch (FormatException fEx)
                {
                    string msg = $"Satır {i + 1} ayrıştırma hatası (Format). Detay: {fEx.Message}. Hata hücreleri: " +
                                 $"Tarih:'{(cells.Length > TarihIndex ? cells[TarihIndex].Trim() : "N/A")}' | " +
                                 $"NetKg:'{(cells.Length > NetKgIndex ? cells[NetKgIndex].Trim() : "N/A")}' | " +
                                 $"Yum. Gramajı:'{(cells.Length > YumurtaGramajiIndex ? cells[YumurtaGramajiIndex].Trim() : "N/A")}' | " +
                                 $"Yum. Adedi:'{(cells.Length > YumurtaAdetIndex ? cells[YumurtaAdetIndex].Trim() : "N/A")}'. " +
                                 $"Satır içeriği: '{lines[i]}'";
                    UpdateStatusText(msg);
                    detayliHataMesaji += msg + Environment.NewLine;
                    hataSayisi++;
                }
                catch (IndexOutOfRangeException ioEx)
                {
                    string msg = $"Satır {i + 1} sütun hatası (İndeks). Detay: {ioEx.Message}. Beklenen en yüksek indeks: {EXPECTED_COLUMN_COUNT - 1}. Bulunan hücre sayısı: {cells.Length}. Satır içeriği: '{lines[i]}'";
                    UpdateStatusText(msg);
                    detayliHataMesaji += msg + Environment.NewLine;
                    hataSayisi++;
                }
                catch (Exception ex)
                {
                    string msg = $"Satır {i + 1} beklenmeyen hata. Detay: {ex.Message}. Satır içeriği: '{lines[i]}'";
                    UpdateStatusText(msg);
                    detayliHataMesaji += msg + Environment.NewLine;
                    hataSayisi++;
                }
            }

            if (yeniAlimlar.Any())
            {
                _yumurtaAlimlari.AddRange(yeniAlimlar);
                SaveYumurtaAlimlari();
                if (yeniTedarikciEklendi)
                {
                    SaveTedarikciler();
                }

                LoadYumurtaAlimlariFromFile();
                MessageBox.Show($"{basariliIslemSayisi} yeni yumurta alımı başarıyla eklendi. {hataSayisi} satırda hata oluştu.\nDetaylı hatalar için Output penceresine bakın.", "İşlem Tamamlandı", MessageBoxButton.OK, MessageBoxImage.Information);
                _selectedFilePath = string.Empty;
                TxtFilePath.Text = string.Empty;

                foreach (Window openWindow in Application.Current.Windows)
                {
                    if (openWindow.GetType().Name == "YumurtaStokWindow")
                    {
                        MethodInfo? method = openWindow.GetType().GetMethod("LoadInitialDataAndSetupFilters", BindingFlags.Public | BindingFlags.Instance);
                        if (method != null)
                        {
                            try
                            {
                                method.Invoke(openWindow, null);
                                MethodInfo? applyFiltersMethod = openWindow.GetType().GetMethod("ApplyFiltersAndCalculateStock", BindingFlags.Public | BindingFlags.Instance);
                                if (applyFiltersMethod != null)
                                {
                                    applyFiltersMethod.Invoke(openWindow, null);
                                }
                                Debug.WriteLine("YumurtaAlimWindow: YumurtaStokWindow (filtreler dahil) reflection ile yenilendi.");
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"YumurtaAlimWindow: YumurtaStokWindow reflection ile yenilenirken hata: {ex.Message}");
                            }
                        }
                        else
                        {
                            Debug.WriteLine("YumurtaAlimWindow: LoadInitialDataAndSetupFilters metodu YumurtaStokWindow'da bulunamadı (Reflection).");
                        }
                    }
                    else if (openWindow.GetType().Name == "IskontoYonetimWindow")
                    {
                        MethodInfo? method = openWindow.GetType().GetMethod("LoadTedarikcilerForComboBox", BindingFlags.Public | BindingFlags.Instance);
                        if (method != null)
                        {
                            try
                            {
                                method.Invoke(openWindow, null);
                                Debug.WriteLine("YumurtaAlimWindow: IskontoYonetimWindow Tedarikçi ComboBox'ı reflection ile yenilendi.");
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"YumurtaAlimWindow: IskontoYonetimWindow reflection ile yenilenirken hata: {ex.Message}");
                            }
                        }
                        else
                        {
                            Debug.WriteLine("YumurtaAlimWindow: LoadTedarikcilerForComboBox metodu IskontoYonetimWindow'da bulunamadı (Reflection).");
                        }
                    }
                }
            }
            else if (hataSayisi > 0)
            {
                MessageBox.Show($"Seçilen dosyadan hiç alım kaydı oluşturulamadı. {hataSayisi} satırda hata oluştu.\nDetaylar:\n{detayliHataMesaji}\nLütfen dosya formatını ve sütun sırasını kontrol edin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Seçilen dosyada işlenecek geçerli veri bulunamadı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TemizleListeyi_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Listede görünen tüm alım kayıtlarını silmek istediğinizden emin misiniz? Bu işlem geri alınamaz.", "Onay", MessageBoxButton.YesNo, MessageBoxImage.Warning); // MessageBoxButton.YesNo parametresi tekrar ediyordu

            if (result == MessageBoxResult.Yes)
            {
                _yumurtaAlimlari.Clear();
                SaveYumurtaAlimlari();
                LoadYumurtaAlimlariFromFile();
                _selectedFilePath = string.Empty;
                TxtFilePath.Text = string.Empty;
                UpdateStatusText("Tüm alım kayıtları temizlendi.");

                foreach (Window openWindow in Application.Current.Windows)
                {
                    if (openWindow.GetType().Name == "YumurtaStokWindow")
                    {
                        MethodInfo? method = openWindow.GetType().GetMethod("LoadInitialDataAndSetupFilters", BindingFlags.Public | BindingFlags.Instance);
                        if (method != null)
                        {
                            try
                            {
                                method.Invoke(openWindow, null);
                                MethodInfo? applyFiltersMethod = openWindow.GetType().GetMethod("ApplyFiltersAndCalculateStock", BindingFlags.Public | BindingFlags.Instance);
                                if (applyFiltersMethod != null)
                                {
                                    applyFiltersMethod.Invoke(openWindow, null);
                                }
                                Debug.WriteLine("YumurtaAlimWindow: YumurtaStokWindow temizleme sonrası reflection ile yenilendi.");
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"YumurtaAlimWindow: YumurtaStokWindow temizleme sonrası reflection ile yenilenirken hata: {ex.Message}");
                            }
                        }
                    }
                    else if (openWindow.GetType().Name == "IskontoYonetimWindow")
                    {
                        MethodInfo? method = openWindow.GetType().GetMethod("LoadTedarikcilerForComboBox", BindingFlags.Public | BindingFlags.Instance);
                        if (method != null)
                        {
                            try
                            {
                                method.Invoke(openWindow, null);
                                Debug.WriteLine("YumurtaAlimWindow: IskontoYonetimWindow Tedarikçi ComboBox'ı temizleme sonrası reflection ile yenilendi.");
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"YumurtaAlimWindow: IskontoYonetimWindow temizleme sonrası reflection ile yenilenirken hata: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }
    }
}