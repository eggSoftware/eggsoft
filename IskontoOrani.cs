using System;
using EggSoft; // YumurtaTipi enum'ına erişmek için gerekli
using System.Text.Json.Serialization; // [JsonIgnore] için gerekli

namespace EggSoft // Projenizin adı 'EggSoft' olduğu için namespace de 'EggSoft' olmalı
{
    public class IskontoOrani
    {
        public int Id { get; set; } // İskonto oranının benzersiz ID'si
        public int TedarikciId { get; set; } // Hangi tedarikçiye ait olduğu (Tedarikci sınıfından ID)
        public YumurtaTipi YumurtaTipi { get; set; } // Hangi yumurta tipi için olduğu (enum'dan)
        public int HaftaNumarasi { get; set; } // İskontonun geçerli olduğu yılın hafta numarası
        public decimal Oran { get; set; } // İskonto oranı (örneğin 0.05 için %5)

        // UI'da göstermek için tedarikçi adı. JSON'a kaydedilmeyecek.
        [JsonIgnore] // Bu attribute, bu özelliğin JSON'a serialize edilmesini engeller.
        public string? TedarikciAd { get; set; } // Null atanabilir olarak işaretlendi

        // İskonto oranını gösterirken kullanışlı olabilecek bir string temsil
        // Örneğin: "Kirli Yumurta için %5.00 (Hafta: 25)"
        public string TamAciklama =>
            $"{TedarikciAd ?? TedarikciId.ToString()} - {YumurtaTipi} için {Oran:P} (Hafta: {HaftaNumarasi})";
        // Not: :P formatı ondalık sayıyı yüzde olarak gösterir (örn: 0.05 -> %5.00)
    }
}