using System.Windows;

namespace EggSoft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Tedarikçi Listesi penceresini açar
        private void TedarikciListesiAc_Click(object sender, RoutedEventArgs e)
        {
            TedarikciListesiWindow tedarikciWindow = new TedarikciListesiWindow();
            tedarikciWindow.Show();
        }

        // İskonto Oranları penceresini açar
        private void IskontoOranlariAc_Click(object sender, RoutedEventArgs e)
        {
            IskontoYonetimWindow iskontoWindow = new IskontoYonetimWindow();
            iskontoWindow.Show();
        }

        // Başmakçı Fiyatları penceresini açar
        private void BasmakciFiyatlariAc_Click(object sender, RoutedEventArgs e)
        {
            BasmakciFiyatYonetimWindow basmakciWindow = new BasmakciFiyatYonetimWindow();
            basmakciWindow.Show();
        }

        // Yumurta Alım penceresini açar
        private void YumurtaAlimAc_Click(object sender, RoutedEventArgs e)
        {
            YumurtaAlimWindow alimWindow = new YumurtaAlimWindow();
            alimWindow.Show();
        }

        // YENİ EKLENEN METOT: Yumurta Stok Durumu penceresini açar
        private void YumurtaStokAc_Click(object sender, RoutedEventArgs e)
        {
            YumurtaStokWindow stokWindow = new YumurtaStokWindow();
            stokWindow.Show();
        }

        // Uygulamadan çıkış yapar
        private void Cikis_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}