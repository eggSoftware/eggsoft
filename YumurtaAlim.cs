using System;
using System.Text.Json.Serialization; // [JsonIgnore] için gerekli

namespace EggSoft // Projenizin adı 'EggSoft' olduğu için namespace de 'EggSoft' olmalı
{
    public class YumurtaAlim
    {
        public int Id { get; set; } // Yumurta alım kaydının benzersiz ID'si
        public DateTime Tarih { get; set; } // Alımın yapıldığı tarih
        public decimal NetKg { get; set; } // Alınan yumurtanın net kilogramı
        public decimal YumurtaGramaji { get; set; } // Ortalama yumurta gramajı (tasnif sonrası)
        public int YumurtaAdedi { get; set; } // Alınan toplam yumurta adedi (örneğin koli bazında veya tek tek)
        public string EtiketNo { get; set; } = string.Empty; // Yumurta partisinin etiket numarası
        public string CariAd { get; set; } = string.Empty; // Tedarikçinin adı (Excel'den gelecek)
        public string StokAd { get; set; } = string.Empty; // Yumurta tipi/sınıfı (Excel'den gelecek, örneğin "Duble Yumurta", "Çatlak Yumurta")

        // UI'da göstermek için tedarikçinin ID'si (opsiyonel, ileride Tedarikci sınıfına bağlamak için)
        // Excel'den direkt gelmeyeceği için JsonIgnore ile işaretliyoruz.
        [JsonIgnore]
        public int? TedarikciId { get; set; }

        // UI'da göstermek için ilgili yumurta tipinin enum değeri (opsiyonel)
        // Excel'den direkt gelmeyeceği için JsonIgnore ile işaretliyoruz.
        [JsonIgnore]
        public YumurtaTipi? YumurtaTipiEnum { get; set; }

        [JsonIgnore]
        public YumurtaSinifi? YumurtaSinifiEnum { get; set; }

        // Kolay okuma için özet bilgi
        public string TamAciklama =>
            $"{Tarih.ToShortDateString()} - {CariAd} ({StokAd}): {YumurtaAdedi} Adet, {NetKg} KG";
    }
}