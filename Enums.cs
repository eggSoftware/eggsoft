namespace EggSoft // Projenizin adı 'EggSoft' olduğu için namespace de 'EggSoft' olmalı
{
    // İskonto oranlarının belirlendiği yumurta tipleri
    public enum YumurtaTipi
    {
        [System.ComponentModel.Description("Kirli Yumurta")] // Daha okunaklı isimler için
        KirliYumurta,
        Catlak,
        [System.ComponentModel.Description("Zar Çatlağı")]
        ZarCatlagi,
        Klavuz // Hem yumurta sınıfı hem tip olarak geçiyor
    }

    // Gramajlarına göre yumurta sınıfları (şimdilik sadece tanım, iskonto oranında kullanılmayacak)
    public enum YumurtaSinifi
    {
        Duble,
        [System.ComponentModel.Description("Eski Ana")]
        EskiAna,
        [System.ComponentModel.Description("Yeni Ana")]
        YeniAna,
        Yarka,
        Pilic,
        Klavuz // Hem yumurta sınıfı hem tip olarak geçiyor
    }
}