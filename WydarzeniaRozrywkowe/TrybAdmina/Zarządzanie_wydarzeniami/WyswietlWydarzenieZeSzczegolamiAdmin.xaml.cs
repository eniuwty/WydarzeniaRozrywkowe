using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WydarzeniaRozrywkowe.BazaDanych;
using WydarzeniaRozrywkowe.TrybUżytkownika;

namespace WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_wydarzeniami
{
    public partial class WyswietlWydarzenieZeSzczegolamiAdmin : Page
    {
        private Wydarzenie wydarzenie;

        public WyswietlWydarzenieZeSzczegolamiAdmin(Wydarzenie wydarzenie)
        {
            InitializeComponent();
            this.wydarzenie = wydarzenie;
            Uzupelnij();
            Button_Cofnij.Focus();
        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;


        public void Uzupelnij()
        {
            NazwaTextBlock.Text = wydarzenie.Nazwa;
            OsobyMaksTextBlock.Text = wydarzenie.Osoby_MAKS.ToString();
            CenaBiletuTextBlock.Text = wydarzenie.Cena_BILETU.ToString("0.00"); // Formatowanie ceny biletu do dwóch miejsc po przecinku
            OpisTextBlock.Text = wydarzenie.OPIS;
            DataTextBlock.Text = wydarzenie.DATA;
            GodzinaTextBlock.Text = wydarzenie.GODZINA;
            MiastoTextBlock.Text = wydarzenie.MIASTO;
            AdresTextBlock.Text = wydarzenie.ADRES;
            KodPocztowyTextBlock.Text = wydarzenie.KOD_POCZTOWY;
            NazwaObiektuTextBlock.Text = wydarzenie.NAZWA_OBIEKTU;

            // Load employees assigned to the event
            List<Pracownik> pracownicyNaWydarzeniu = new List<Pracownik>();
            string connectionString = "Data Source=Wydarzenia.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT p.Id_Pracownik, p.Imie, p.Nazwisko " +
                               "FROM Pracownik p " +
                               "JOIN PracownicyNaWydarzeniu pw ON p.Id_Pracownik = pw.Id_Pracownika " +
                               "WHERE pw.Id_Wydarzenia = @IdWydarzenia";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdWydarzenia", wydarzenie.Id_Wydarzenia);
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
                            pracownicyNaWydarzeniu.Add(pracownik);
                        }
                    }
                }
            }

            // Bind the list of employees to the ListView
            ListViewPracownicyNaWydarzeniu.ItemsSource = pracownicyNaWydarzeniu;
        }

        private void Button_Cofnij_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzeniaAdmin());
            mainWindow.labelStatus.Content = "Jesteś w: Wyświetlanie wydarzeń";
            //WyswietlWydarzeniaAdmin wyswietlWydarzenia = new WyswietlWydarzeniaAdmin();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = wyswietlWydarzenia;
        }

        private void WyswietlWydarzeniaZeSzczegolami_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Button_Cofnij_Click(this, new RoutedEventArgs());
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new EdytujWydarzenie(
                    wydarzenie.Id_Wydarzenia,
                    wydarzenie.Nazwa,
                    wydarzenie.OPIS,
                    wydarzenie.Osoby_MAKS,
                    wydarzenie.Cena_BILETU,
                    wydarzenie.DATA,
                    wydarzenie.GODZINA,
                    wydarzenie.MIASTO,
                    wydarzenie.ADRES,
                    wydarzenie.KOD_POCZTOWY,
                    wydarzenie.NAZWA_OBIEKTU));
                
            mainWindow.labelStatus.Content = "Jesteś w: Edytowanie wydarzenia";
        }
    }
}
