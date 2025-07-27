using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Globalization;

using EggSoft;

namespace EggSoft
{
    /// <summary>
    /// Interaction logic for YumurtaStokWindow.xaml
    /// </summary>
    public partial class YumurtaStokWindow : Window
    {
        private const string YUMURTA_ALIMLARI_FILE_PATH = "yumurtaAlimlari.json";
        private const string TEDARIKCILER_FILE_PATH = "tedarikciler.json";

        private List<YumurtaAlim> _tumAlimlar;
        private List<Tedarikci> _tumTedarikciler;

        public YumurtaStokWindow()
        {
            InitializeComponent();
            _tumAlimlar = new List<YumurtaAlim>();
            _tumTedarikciler = new List<Tedarikci>();

            LoadInitialDataAndSetupFilters();
            ApplyFiltersAndCalculateStock();
        }

        public void LoadInitialDataAndSetupFilters()
        {
            Debug.WriteLine("YumurtaStokWindow: Başlangıç verileri ve filtreler yükleniyor...");

            // Tüm Alım verilerini yükle
            Debug.WriteLine($"YumurtaStokWindow: Alım dosyası kontrol ediliyor: {Path.GetFullPath(YUMURTA_ALIMLARI_FILE_PATH)}");
            if (File.Exists(YUMURTA_ALIMLARI_FILE_PATH))
            {
                try
                {
                    string jsonString = File.ReadAllText(YUMURTA_ALIMLARI_FILE_PATH);
                    _tumAlimlar = JsonSerializer.Deserialize<List<YumurtaAlim>>(jsonString) ?? new List<YumurtaAlim>();
                    Debug.WriteLine($"YumurtaStokWindow: '{YUMURTA_ALIMLARI_FILE_PATH}' dosyasından {_tumAlimlar.Count} alım kaydı yüklendi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Yumurta alım verileri yüklenirken bir hata oluştu: {ex.Message}\nStok hesaplaması boş liste ile devam ediliyor.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    _tumAlimlar = new List<YumurtaAlim>();
                    Debug.WriteLine($"YumurtaStokWindow: HATA: Yumurta alım verileri yüklenirken: {ex.Message}");
                }
            }
            else
            {
                _tumAlimlar = new List<YumurtaAlim>();
                Debug.WriteLine($"YumurtaStokWindow: UYARI: '{YUMURTA_ALIMLARI_FILE_PATH}' dosyası bulunamadı.");
            }

            // Tüm Tedarikçi verilerini yükle (ComboBox için)
            Debug.WriteLine($"YumurtaStokWindow: Tedarikçi dosyası kontrol ediliyor: {Path.GetFullPath(TEDARIKCILER_FILE_PATH)}");
            if (File.Exists(TEDARIKCILER_FILE_PATH))
            {
                try
                {
                    string jsonString = File.ReadAllText(TEDARIKCILER_FILE_PATH);
                    _tumTedarikciler = JsonSerializer.Deserialize<List<Tedarikci>>(jsonString) ?? new List<Tedarikci>();
                    Debug.WriteLine($"YumurtaStokWindow: '{TEDARIKCILER_FILE_PATH}' dosyasından {_tumTedarikciler.Count} tedarikçi yüklendi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Tedarikçi verileri yüklenirken bir hata oluştu: {ex.Message}\nTedarikçi filtresi boş olacak.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    _tumTedarikciler = new List<Tedarikci>();
                    Debug.WriteLine($"YumurtaStokWindow: HATA: Tedarikçi verileri yüklenirken: {ex.Message}");
                }
            }
            else
            {
                _tumTedarikciler = new List<Tedarikci>();
                MessageBox.Show("Tedarikçi verileri dosyası (tedarikciler.json) bulunamadı. Lütfen önce Tedarikçi Listesine tedarikçi ekleyin.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                Debug.WriteLine($"YumurtaStokWindow: UYARI: '{TEDARIKCILER_FILE_PATH}' dosyası bulunamadı.");
            }

            // Tedarikçi ComboBox'ını doldur
            List<Tedarikci> tedarikciFiltreListesi = new List<Tedarikci>();
            tedarikciFiltreListesi.Add(new Tedarikci { Id = 0, Ad = "Tümü" });
            tedarikciFiltreListesi.AddRange(_tumTedarikciler.OrderBy(t => t.Ad));
            CmbTedarikci.ItemsSource = tedarikciFiltreListesi;
            CmbTedarikci.DisplayMemberPath = "Ad";
            CmbTedarikci.SelectedValuePath = "Id";
            CmbTedarikci.SelectedIndex = 0;
            Debug.WriteLine($"YumurtaStokWindow: Tedarikçi ComboBox'ına yüklenen eleman sayısı: {tedarikciFiltreListesi.Count}");


            // Yumurta Tipi/Sınıfı ComboBox'ını doldur
            var yumurtaTipleriSiniflar = new List<string>();
            yumurtaTipleriSiniflar.Add("Tümü");
            yumurtaTipleriSiniflar.AddRange(Enum.GetNames(typeof(YumurtaTipi)).Select(name => (YumurtaTipi)Enum.Parse(typeof(YumurtaTipi), name)).Select(e => e.ToString()));
            yumurtaTipleriSiniflar.AddRange(Enum.GetNames(typeof(YumurtaSinifi)).Select(name => (YumurtaSinifi)Enum.Parse(typeof(YumurtaSinifi), name)).Where(s => s != YumurtaSinifi.Klavuz).Select(e => e.ToString()));
            CmbYumurtaTipSinif.ItemsSource = yumurtaTipleriSiniflar.OrderBy(s => s).ToList();
            CmbYumurtaTipSinif.SelectedIndex = 0;

            DpBitisTarihi.SelectedDate = null;
            DpBaslangicTarihi.SelectedDate = null;
            TxtHaftaNo.Clear();
            TxtYil.Clear();

            UpdateStatusText("Veriler yüklendi ve filtreler hazırlandı.");
            Debug.WriteLine("YumurtaStokWindow: Başlangıç veri yükleme ve filtre kurulumu tamamlandı.");
        }

        public void ApplyFiltersAndCalculateStock()
        {
            Debug.WriteLine("YumurtaStokWindow: Stoklar filtrelenip hesaplanıyor...");

            IEnumerable<YumurtaAlim> filtrelenmisAlimlar = _tumAlimlar;
            Debug.WriteLine($"YumurtaStokWindow: Başlangıç filtrelenmiş alım sayısı: {filtrelenmisAlimlar.Count()}");

            // 1. Tarih Aralığı Filtresi
            if (DpBaslangicTarihi.SelectedDate.HasValue)
            {
                DateTime baslangic = DpBaslangicTarihi.SelectedDate.Value.Date;
                filtrelenmisAlimlar = filtrelenmisAlimlar.Where(a => a.Tarih.Date >= baslangic);
                Debug.WriteLine($"YumurtaStokWindow: Tarih Başlangıç Filtresi uygulandı: {baslangic.ToShortDateString()}. Kalan alım: {filtrelenmisAlimlar.Count()}");
            }
            if (DpBitisTarihi.SelectedDate.HasValue)
            {
                DateTime bitis = DpBitisTarihi.SelectedDate.Value.Date.AddDays(1).AddMilliseconds(-1);
                filtrelenmisAlimlar = filtrelenmisAlimlar.Where(a => a.Tarih <= bitis);
                Debug.WriteLine($"YumurtaStokWindow: Tarih Bitiş Filtresi uygulandı: {bitis.ToShortDateString()}. Kalan alım: {filtrelenmisAlimlar.Count()}");
            }

            // 2. Hafta Numarası ve Yıl Filtresi
            int haftaNo = 0;
            int yil = 0;
            bool haftaNoGecerli = int.TryParse(TxtHaftaNo.Text, out haftaNo) && haftaNo >= 1 && haftaNo <= 53;
            bool yilGecerli = int.TryParse(TxtYil.Text, out yil) && yil >= 2000 && yil <= 9999;

            if (haftaNoGecerli && yilGecerli)
            {
                System.Globalization.Calendar cal = CultureInfo.CurrentCulture.Calendar;
                filtrelenmisAlimlar = filtrelenmisAlimlar.Where(a =>
                    cal.GetWeekOfYear(a.Tarih, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) == haftaNo &&
                    a.Tarih.Year == yil);
                Debug.WriteLine($"YumurtaStokWindow: Hafta/Yıl Filtresi uygulandı: Hafta {haftaNo}, Yıl {yil}. Kalan alım: {filtrelenmisAlimlar.Count()}");
            }
            else if (!string.IsNullOrWhiteSpace(TxtHaftaNo.Text) || !string.IsNullOrWhiteSpace(TxtYil.Text))
            {
                if (!haftaNoGecerli && !string.IsNullOrWhiteSpace(TxtHaftaNo.Text))
                {
                    MessageBox.Show("Hafta Numarası alanı geçerli bir sayı (1-53) olmalıdır.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!yilGecerli && !string.IsNullOrWhiteSpace(TxtYil.Text))
                {
                    MessageBox.Show("Yıl alanı geçerli bir sayı olmalıdır (örn: 2024).", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }


            // 3. Yumurta Tipi/Sınıfı Filtresi
            if (CmbYumurtaTipSinif.SelectedItem != null && CmbYumurtaTipSinif.SelectedItem.ToString() != "Tümü")
            {
                string secilenTipSinifAdi = CmbYumurtaTipSinif.SelectedItem.ToString();
                filtrelenmisAlimlar = filtrelenmisAlimlar.Where(a => a.StokAd.Contains(secilenTipSinifAdi, StringComparison.OrdinalIgnoreCase));
                Debug.WriteLine($"YumurtaStokWindow: Yumurta Tipi/Sınıfı Filtresi uygulandı: {secilenTipSinifAdi}. Kalan alım: {filtrelenmisAlimlar.Count()}");
            }

            // 4. Tedarikçi Filtresi
            if (CmbTedarikci.SelectedValue != null && (int)CmbTedarikci.SelectedValue != 0)
            {
                int tedarikciId = (int)CmbTedarikci.SelectedValue;
                Tedarikci? secilenTedarikci = _tumTedarikciler.FirstOrDefault(t => t.Id == tedarikciId);
                if (secilenTedarikci != null)
                {
                    string tedarikciAdi = secilenTedarikci.Ad;
                    filtrelenmisAlimlar = filtrelenmisAlimlar.Where(a => a.CariAd.Equals(tedarikciAdi, StringComparison.OrdinalIgnoreCase));
                    Debug.WriteLine($"YumurtaStokWindow: Tedarikçi Filtresi uygulandı: {tedarikciAdi}. Kalan alım: {filtrelenmisAlimlar.Count()}");
                }
            }


            // Filtrelenmiş alımlardan stokları hesapla (kabuk oranı dahil)
            var hesaplananStoklar = filtrelenmisAlimlar
                .GroupBy(a => a.StokAd)
                .Select(g =>
                {
                    decimal toplamBrutKg = g.Sum(a => a.NetKg);
                    int toplamAdet = g.Sum(a => a.YumurtaAdedi);

                    string stokAdi = g.Key;

                    decimal kabukOrani = 0.15m;

                    if (stokAdi.Contains("Klavuz", StringComparison.OrdinalIgnoreCase) ||
                        stokAdi.Contains("Pilic", StringComparison.OrdinalIgnoreCase) ||
                        stokAdi.Contains("PİLİÇ", StringComparison.OrdinalIgnoreCase))
                    {
                        kabukOrani = 0.16m;
                    }

                    decimal mevcutKabuksuzKg = toplamBrutKg * (1m - kabukOrani);

                    return new YumurtaStok
                    {
                        Id = 0,
                        StokAdi = stokAdi,
                        MevcutAdet = toplamAdet,
                        MevcutKg = mevcutKabuksuzKg
                    };
                })
                .OrderBy(s => s.StokAdi)
                .ToList();

            Debug.WriteLine($"YumurtaStokWindow: Hesaplanan farklı stok tipi sayısı (filtre sonrası): {hesaplananStoklar.Count}. Toplam eşleşen alım kaydı: {filtrelenmisAlimlar.Count()}");
            foreach (var stok in hesaplananStoklar)
            {
                Debug.WriteLine($"  Stok: {stok.StokAdi}, Adet: {stok.MevcutAdet}, Kabuksuz Kg: {stok.MevcutKg:N2}");
            }

            YumurtaStokDataGrid.ItemsSource = null;
            YumurtaStokDataGrid.ItemsSource = hesaplananStoklar;
            UpdateStatusText($"{hesaplananStoklar.Count} farklı yumurta stoğu yüklendi (filtre sonrası). Toplam eşleşen alım kaydı: {filtrelenmisAlimlar.Count()}");
            Debug.WriteLine("YumurtaStokWindow: Stoklar hesaplama ve yükleme tamamlandı.");
        }

        private void Filtrele_Click(object sender, RoutedEventArgs e)
        {
            ApplyFiltersAndCalculateStock();
        }

        private void TemizleFiltreler_Click(object sender, RoutedEventArgs e)
        {
            DpBaslangicTarihi.SelectedDate = null;
            DpBitisTarihi.SelectedDate = null;
            CmbYumurtaTipSinif.SelectedIndex = 0;
            CmbTedarikci.SelectedIndex = 0;
            TxtHaftaNo.Clear();
            TxtYil.Clear();
            ApplyFiltersAndCalculateStock();
            UpdateStatusText("Filtreler temizlendi, tüm stoklar gösteriliyor.");
        }

        private void TedarikcileriYenile_Click(object sender, RoutedEventArgs e)
        {
            LoadInitialDataAndSetupFilters();
            ApplyFiltersAndCalculateStock();
            UpdateStatusText("Tedarikçi listesi ve stoklar güncellendi.");
        }

        private void StoklariYenile_Click(object sender, RoutedEventArgs e)
        {
            LoadInitialDataAndSetupFilters();
            ApplyFiltersAndCalculateStock();
            UpdateStatusText("Stok durumu ve veriler yenilendi.");
        }

        private void UpdateStatusText(string message)
        {
            StatusTextBlock.Text = message;
        }
    }
}