using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WydarzeniaRozrywkowe.BazaDanych;
using WydarzeniaRozrywkowe.Nawigacja;

namespace WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_wydarzeniami
{
    public partial class DodawaniePracownikowDoWydarzenia : Page
    {
        
        int ludziska = 0;
        int id_wydarzenia;

        private List<Pracownik> dostepniPracownicy;
        private List<Pracownik> dodaniPracownicy;

        public DodawaniePracownikowDoWydarzenia(int id_wydarzenia)
        {
            InitializeComponent();
            this.id_wydarzenia = id_wydarzenia;
            WczytajDane();
            Button_Cofnij.Focus();
            Esc.Ludziska = 0;
        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;


        private void WczytajDane()
        {
            List<Pracownik> wszyscyPracownicy = PobierzWszystkichPracownikow();
            dodaniPracownicy = PobierzPracownikowNaWydarzeniu(id_wydarzenia);
            dostepniPracownicy = wszyscyPracownicy.Except(dodaniPracownicy).ToList();

            ListViewListaPracownikow.ItemsSource = dostepniPracownicy;
            ListViewDodaniPracownicy.ItemsSource = dodaniPracownicy;
        }

        private List<Pracownik> PobierzWszystkichPracownikow()
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
                            var id_pracownika = reader["Id_Pracownik"];
                            var imie = reader["Imie"];
                            var nazwisko = reader["Nazwisko"];
                            pracownicy.Add(new Pracownik(Convert.ToInt32(id_pracownika), imie.ToString(), nazwisko.ToString()));
                        }
                    }
                }
            }
            return pracownicy;
        }

        private List<Pracownik> PobierzPracownikowNaWydarzeniu(int id_wydarzenia)
        {
            List<Pracownik> pracownicy = new List<Pracownik>();
            string connectionString = "Data Source=Wydarzenia.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT Pracownik.* FROM PracownicyNaWydarzeniu INNER JOIN Pracownik ON PracownicyNaWydarzeniu.Id_Pracownika = Pracownik.Id_Pracownik WHERE Id_Wydarzenia = @IdWydarzenia", connection))
                {
                    command.Parameters.AddWithValue("@IdWydarzenia", id_wydarzenia);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id_pracownika = reader["Id_Pracownik"];
                            var imie = reader["Imie"];
                            var nazwisko = reader["Nazwisko"];
                            pracownicy.Add(new Pracownik(Convert.ToInt32(id_pracownika), imie.ToString(), nazwisko.ToString()));
                        }
                    }
                }
            }
            return pracownicy;
        }

        private void ButtonDodawanieClick(object sender, RoutedEventArgs e)
        {
            if (ListViewListaPracownikow.SelectedItem is Pracownik pracownik)
            {
                DodajPracownikaDoWydarzenia(pracownik);
                // Usuwanie pracownika z listy dostępnych i dodawanie do listy dodanych
                dostepniPracownicy.Remove(pracownik);
                dodaniPracownicy.Add(pracownik);
                // Aktualizacja źródła danych
                ListViewListaPracownikow.ItemsSource = null;
                ListViewListaPracownikow.ItemsSource = dostepniPracownicy;
                ListViewDodaniPracownicy.ItemsSource = null;
                ListViewDodaniPracownicy.ItemsSource = dodaniPracownicy;

                MessageBox.Show("Dodano nowego pracownika do wydarzenia");
            }
        }

        private void ButtonUsuwanieClick(object sender, RoutedEventArgs e)
        {
            if (ListViewDodaniPracownicy.SelectedItem is Pracownik pracownik)
            {
                UsunPracownikaZWydarzenia(pracownik);
                // Usuwanie pracownika z listy dodanych i dodawanie do listy dostępnych
                dodaniPracownicy.Remove(pracownik);
                dostepniPracownicy.Add(pracownik);
                // Aktualizacja źródła danych
                ListViewListaPracownikow.ItemsSource = null;
                ListViewListaPracownikow.ItemsSource = dostepniPracownicy;
                ListViewDodaniPracownicy.ItemsSource = null;
                ListViewDodaniPracownicy.ItemsSource = dodaniPracownicy;

                MessageBox.Show("Usunięto pracownika z wydarzenia");
            }
        }

        private void DodajPracownikaDoWydarzenia(Pracownik pracownik)
        {
            string connectionString = "Data Source=Wydarzenia.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO PracownicyNaWydarzeniu (Id_Pracownika, Id_Wydarzenia) VALUES (@IdPracownika, @IdWydarzenia)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@IdPracownika", pracownik.Id_Pracownik);
                    command.Parameters.AddWithValue("@IdWydarzenia", id_wydarzenia);
                    command.ExecuteNonQuery();
                }
            }
            ludziska++;
            Esc.Ludziska++;
        }

        private void UsunPracownikaZWydarzenia(Pracownik pracownik)
        {
            string connectionString = "Data Source=Wydarzenia.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM PracownicyNaWydarzeniu WHERE Id_Pracownika = @IdPracownika AND Id_Wydarzenia = @IdWydarzenia";
                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@IdPracownika", pracownik.Id_Pracownik);
                    command.Parameters.AddWithValue("@IdWydarzenia", id_wydarzenia);
                    command.ExecuteNonQuery();
                }
            }
            ludziska--;
            Esc.Ludziska--;
        }

        private void ButtonCofnij(object sender, RoutedEventArgs e)
        {
            if (ludziska == 0)
            {
                MessageBoxResult result1 = MessageBox.Show(
                    "Czy na pewno nie chcesz dodać żadnego pracownika do obsługi wydarzenia?",
                    "Uwaga",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    PowrotDoMenuAdmina();
                }
            }
            else
            {
                ludziska = -1;
                Esc.Ludziska =-1;
                PowrotDoMenuAdmina();
            }
        }

        private void PowrotDoMenuAdmina()
        {
            mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzeniaAdmin());
            mainWindow.labelStatus.Content = "Jesteś w: Wyświetlanie wydarzeń";
            //TrybAdminaMenu trybAdminaMenu = new TrybAdminaMenu();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = trybAdminaMenu;
        }

        private void DodawaniePracownikowDoWYdarzenia_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ButtonCofnij(this, new RoutedEventArgs());
            }
            if (e.Key == Key.Enter)
            {
                ButtonDodawanieClick(this, new RoutedEventArgs());
            }
        }

        private void ListViewListaPracownikow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewListaPracownikow.SelectedItem != null)
            {
                ButtonDodawanieClick(this, new RoutedEventArgs());
            }
        }

        private void ListViewDodaniPracownicy_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewDodaniPracownicy.SelectedItem != null)
            {
                ButtonUsuwanieClick(this, new RoutedEventArgs());
            }
        }

        private void ButtonGotowe_Click(object sender, RoutedEventArgs e)
        {
            ButtonCofnij(this, new RoutedEventArgs());

        }
    }
}
