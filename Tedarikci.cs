namespace EggSoft // Projenizin adı 'EggSoft' olduğu için namespace de 'EggSoft' olmalı
{
    public class Tedarikci
    {
        public int Id { get; set; } // Tedarikçinin benzersiz ID'si
        public string Ad { get; set; } = string.Empty; // Tedarikçi Adı
        public string Sehir { get; set; } = string.Empty; // Tedarikçinin Şehri
        public string Telefon { get; set; } = string.Empty; // Tedarikçinin Telefon Numarası
    }
}