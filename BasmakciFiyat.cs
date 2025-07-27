using System;

namespace EggSoft // Projenizin adı 'EggSoft' olduğu için namespace de 'EggSoft' olmalı
{
    public class BasmakciFiyat
    {
        public int Id { get; set; } // Fiyat kaydının benzersiz ID'si
        public DateTime Tarih { get; set; } // Fiyatın açıklandığı tarih
        public decimal DubleYumurtaFiyati { get; set; } // Duble yumurta için açıklanan fiyat
        public int HaftaNumarasi { get; set; } // Fiyatın ait olduğu hafta numarası
    }
}