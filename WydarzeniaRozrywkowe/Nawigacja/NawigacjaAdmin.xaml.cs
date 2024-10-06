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
using WydarzeniaRozrywkowe;
using WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_Pracownikami;
using WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_wydarzeniami;

namespace WydarzeniaRozrywkowe.Nawigacja
{
    /// <summary>
    /// Logika interakcji dla klasy NawigacjaAdmin.xaml
    /// </summary>
    public partial class NawigacjaAdmin : Page
    {

        public NawigacjaAdmin()
        {
            InitializeComponent();
        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private void ButtonWyloguj_Click(object sender, RoutedEventArgs e)
        {
            if(Esc.Ludziska ==-1)
            {

            mainWindow.labelzalogowano.Content = "Zalogowano jako: Gość";
            mainWindow.labelStatus.Content = "Jesteś w: Strona Główna";
            mainWindow.MainFrame.NavigationService.Navigate(new glowna());
            mainWindow.NavigationFrame.NavigationService.Navigate(new NawigacjaUzytkownik());
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Czy na pewno chcesz przerwać dodawanie pracowników?",
                    "Uwaga",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    mainWindow.labelzalogowano.Content = "Zalogowano jako: Gość";
                    mainWindow.labelStatus.Content = "Jesteś w: Strona Główna";
                    mainWindow.MainFrame.NavigationService.Navigate(new glowna());
                    mainWindow.NavigationFrame.NavigationService.Navigate(new NawigacjaUzytkownik());
                    Esc.Ludziska = -1;

                }

            }
        }

        private void ButtonZarzadzanieWydarzeniami_Click(object sender, RoutedEventArgs e)
        {
            if (Esc.Ludziska == -1)
            {
                mainWindow.labelStatus.Content = "Jesteś w: Wyświetlanie wydarzeń";
                mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzeniaAdmin());
                //WyswietlWydarzeniaAdmin wyswietlWydarzenia = new WyswietlWydarzeniaAdmin();
                //Window parentWindow = Window.GetWindow(this);
                //parentWindow.Content = wyswietlWydarzenia;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Czy na pewno chcesz przerwać dodawanie pracowników?",
                    "Uwaga",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    mainWindow.labelStatus.Content = "Jesteś w: Wyświetlanie wydarzeń";
                    mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzeniaAdmin());
                    Esc.Ludziska = -1;
                }

            }

        }

        private void ButtonZarzadzaniePracownikami_Click(object sender, RoutedEventArgs e)
        {
            if (Esc.Ludziska == -1)
            {
                mainWindow.labelStatus.Content = "Jesteś w: Lista pracowników";
                mainWindow.MainFrame.NavigationService.Navigate(new ListaPracowników());
                // ListaPracowników listaPracowników = new ListaPracowników();
                //Window parentWindow = Window.GetWindow(this);
                //parentWindow.Content = listaPracowników;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                "Czy na pewno chcesz przerwać dodawanie pracowników?",
                "Uwaga",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    mainWindow.labelStatus.Content = "Jesteś w: Lista pracowników";
                    mainWindow.MainFrame.NavigationService.Navigate(new ListaPracowników());
                    Esc.Ludziska = -1;
                }

            }

        }

        private void ButtonGlowna_Click(object sender, RoutedEventArgs e)
        {
            if (Esc.Ludziska == -1)
            {
                mainWindow.MainFrame.NavigationService.Navigate(new glowna_admin());
                mainWindow.labelStatus.Content = "Jesteś w: Strona główna";
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                "Czy na pewno chcesz przerwać dodawanie pracowników?",
                "Uwaga",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    mainWindow.MainFrame.NavigationService.Navigate(new glowna_admin());
                    mainWindow.labelStatus.Content = "Jesteś w: Strona główna";
                    Esc.Ludziska = -1;
                }
            }
        }
    }
}
