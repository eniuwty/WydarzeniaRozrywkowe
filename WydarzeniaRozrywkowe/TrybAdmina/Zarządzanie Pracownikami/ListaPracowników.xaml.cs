using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using WydarzeniaRozrywkowe.BazaDanych;
using WydarzeniaRozrywkowe.Nawigacja;

namespace WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_Pracownikami
{
    /// <summary>
    /// Logika interakcji dla klasy ListaPracowników.xaml
    /// </summary>
    public partial class ListaPracowników : Page
    {
        public ListaPracowników()
        {
            InitializeComponent();
            WczytajDane();
            Button_Cofnij.Focus();
        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        private void WczytajDane()
        {
            List<Pracownik> pracownicy = new List<Pracownik>();

            string connectionString = "Data Source=Wydarzenia.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT * FROM Pracownik", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = reader["Id_Pracownik"];
                            var imie = reader["Imie"];
                            var nazwisko = reader["Nazwisko"];
                            int id_int = Convert.ToInt32(id);
                            string imies = imie.ToString();
                            string nazwiskos = nazwisko.ToString();
                            Pracownik pracownik = new Pracownik(id_int, imies, nazwiskos);
                            pracownicy.Add(pracownik);
                        }
                    }
                }
            }

            // Przypisz listę pracowników do ItemsSource ListView
            ListViewListaPracownikow.ItemsSource = pracownicy;
            // Odśwież ListView
            ListViewListaPracownikow.Items.Refresh();
        }

        private void UsunPracownikaZBazyDanych(int idPracownika)
        {
            try
            {
                string connectionString = "Data Source=Wydarzenia.db;Version=3;";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand("DELETE FROM Pracownik WHERE Id_Pracownik = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", idPracownika);
                        command.ExecuteNonQuery();
                    }
                }
                // Powiadomienie o sukcesie usuwania (np. MessageBox lub inny komunikat)
                MessageBox.Show("Pracownik został usunięty z bazy danych.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Obsługa błędów (np. wyświetlenie komunikatu o błędzie)
                MessageBox.Show($"Wystąpił błąd podczas usuwania pracownika: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonUsunClick(object sender, RoutedEventArgs e)
        {

            if(ListViewListaPracownikow.SelectedItem is Pracownik pracownik)
            {

                MessageBoxResult result = MessageBox.Show(
                "Czy na pewno chcesz usunąć pracownika z bazy danych?",
                "Uwaga",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    
                        int idPracownika = pracownik.Id_Pracownik;
                        // Wywołaj metodę usuwającą pracownika z bazy danych na podstawie idPracownika
                        UsunPracownikaZBazyDanych(idPracownika);

                        // Odśwież listę pracowników
                        WczytajDane();
                    
                }
            }




        }

        private void Dodawanie(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new DodajPracownika());
            mainWindow.labelStatus.Content = "Jesteś w: Dodawanie pracowników";
            //DodajPracownika dodajPracownika = new DodajPracownika();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = dodajPracownika;
        }

        private void ButtonPowortDoMenu(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new glowna_admin());
            mainWindow.labelStatus.Content = "Jesteś w: Strona główna";
          //  mainWindow.MainFrame.NavigationService.Navigate(new ListaPracowników());
            //TrybAdminaMenu trybAdminaMenu = new TrybAdminaMenu();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = trybAdminaMenu;
        }
        private void ListaPracownikow_Esc(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Escape)
            {
                ButtonPowortDoMenu(this, new RoutedEventArgs());
            }
            if(e.Key == System.Windows.Input.Key.Back || e.Key == System.Windows.Input.Key.Delete && ListViewListaPracownikow.SelectedItem is Pracownik pracownik)
            {
                ButtonUsunClick(this,new RoutedEventArgs());
            }
        }
    }
}

