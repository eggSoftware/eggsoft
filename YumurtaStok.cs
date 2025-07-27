using System;
using EggSoft; // YumurtaTipi ve YumurtaSinifi enum'larına erişmek için

namespace EggSoft // Projenizin adı 'EggSoft' olduğu için namespace de 'EggSoft' olmalı
{
    public class YumurtaStok
    {
        public int Id { get; set; } // Stok kaydının benzersiz ID'si

        // Yumurta Tipi (örneğin Çatlak, Kirli Yumurta)
        public YumurtaTipi? YumurtaTipi { get; set; }

        // Yumurta Sınıfı (örneğin Duble, Jumbo)
        public YumurtaSinifi? YumurtaSinifi { get; set; }

        // Adet bazında stok
        public int MevcutAdet { get; set; }

        // Kilogram bazında stok
        public decimal MevcutKg { get; set; }

        // UI'da veya raporlarda gösterim için açıklama - ARTIK YAZILABİLİR OLARAK DÜZELTİLDİ
        public string StokAdi { get; set; } = string.Empty; // Varsayılan değer eklendi

        // İleride son güncelleme tarihi gibi alanlar da eklenebilir
        // public DateTime SonGuncellemeTarihi { get; set; }
    }
}