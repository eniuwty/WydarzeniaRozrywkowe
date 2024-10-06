using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WydarzeniaRozrywkowe.TrybUżytkownika;
using WydarzeniaRozrywkowe;
using WydarzeniaRozrywkowe.TrybAdmina;



namespace WydarzeniaRozrywkowe.Nawigacja
{
    /// <summary>
    /// Logika interakcji dla klasy NawigacjaUzytkownik.xaml
    /// </summary>
    public partial class NawigacjaUzytkownik : Page
    {
       // private MainWindow _MainWindow;
        public NawigacjaUzytkownik()
        {
            InitializeComponent();
          //  _MainWindow = new MainWindow();
        }
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        private void ButtonNawigacja1_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzenia());
            mainWindow.labelStatus.Content = "Jesteś w: Wydarzenia";
        }

        private void ButtonGlowna_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new glowna());
            mainWindow.labelStatus.Content = "Jesteś w: Strona Główna";
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
           // logowanie logowanie = new logowanie();
            mainWindow.MainFrame.NavigationService.Navigate(new logowanie());
            mainWindow.labelStatus.Content = "Jesteś w: Logowanie się";
            //logowanie.ShowDialog();
            /*
            if(Esc.logowanie==1)
            {
                mainWindow.labelzalogowano.Content = "Zalogowano jako: Admin";
                mainWindow.labelStatus.Content = "Jesteś w: Strona Główna";
                mainWindow.NavigationFrame.NavigationService.Navigate(new NawigacjaAdmin());
                mainWindow.MainFrame.NavigationService.Navigate(new glowna_admin());
            }
            */
        }
    }
}
