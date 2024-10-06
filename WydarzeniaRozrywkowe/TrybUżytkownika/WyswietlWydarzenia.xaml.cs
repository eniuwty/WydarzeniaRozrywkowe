using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WydarzeniaRozrywkowe.BazaDanych;
using WydarzeniaRozrywkowe.Nawigacja;
using WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_Pracownikami;

namespace WydarzeniaRozrywkowe.TrybUżytkownika
{
    /// <summary>
    /// Logika interakcji dla klasy WyswietlWydarzenia.xaml
    /// </summary>
    /// 

    public partial class WyswietlWydarzenia : Page
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        private List<Wydarzenie> wydarzenia = new List<Wydarzenie>();

        public WyswietlWydarzenia()
        {
            InitializeComponent();
            WczytajWydarzenia();
            ButtonSzczegoly.Focus();
        }

        public void WczytajWydarzenia()
        {
            string connectionString = "Data Source=Wydarzenia.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT * FROM Wydarzenie", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id_Wydarzenia"]);
                            string nazwa = reader["Nazwa"].ToString();
                            int osobyMaks = Convert.ToInt32(reader["Osoby_MAKS"]);
                            double cenaBiletu = Convert.ToDouble(reader["Cena_BILETU"]);
                            string opis = reader["OPIS"].ToString();
                            string data = reader["DATA"].ToString();
                            string godzina = reader["GODZINA"].ToString();
                            string miasto = reader["MIASTO"].ToString();
                            string adres = reader["ADRES"].ToString();
                            string kodPocztowy = reader["KOD_POCZTOWY"].ToString();
                            string nazwaObiektu = reader["NAZWA_OBIEKTU"].ToString();

                            Wydarzenie wydarzenie = new Wydarzenie(id, nazwa, osobyMaks, cenaBiletu, opis, data, godzina, miasto, adres, kodPocztowy, nazwaObiektu);
                            wydarzenia.Add(wydarzenie);
                        }
                    }
                }
                listBox.ItemsSource = wydarzenia;
            }
        }

        
        private void ButtonPowrotDoMenu(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new glowna());
            mainWindow.labelStatus.Content = "Jesteś w: Strona główna";

            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Close();
        }

        private void WyswietlWYdarzenia_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ButtonPowrotDoMenu(this, new RoutedEventArgs());
            }

            if (e.Key == Key.Enter)
            {
                Button_SzczegolyClick(this, new RoutedEventArgs());
            }
        }

        private void ButtonFiltrujPoDacie_Click(object sender, RoutedEventArgs e)
        {
            var sortedWydarzenia = wydarzenia
                .Where(w =>
                {
                    DateTime data;
                    TimeSpan godzina;
                    bool validData = DateTime.TryParse(w.DATA, out data);
                    bool validGodzina = TimeSpan.TryParse(w.GODZINA, out godzina);
                    return validData && validGodzina;
                })
                .OrderBy(w =>
                {
                    DateTime data = DateTime.Parse(w.DATA);
                    TimeSpan godzina = TimeSpan.Parse(w.GODZINA);
                    return data + godzina;
                })
                .ToList();

            listBox.ItemsSource = sortedWydarzenia;
        }

        private void ButtonFiltrujPoNazwie_Click(object sender, RoutedEventArgs e)
        {
            var sortedWydarzenia = wydarzenia
                .OrderBy(w => w.Nazwa)
                .ToList();

            listBox.ItemsSource = sortedWydarzenia;
        }

        private void Button_SzczegolyClick(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem is Wydarzenie wydarzenie)
            {
                
                Wydarzenie wybraneWydarzenie = (Wydarzenie)listBox.SelectedItem;
                //WydarzenieZeSzczegolami wydarzenieZeSzczegolami = new WydarzenieZeSzczegolami(wybraneWydarzenie);
                //Window parentWindow = Window.GetWindow(this);
                // parentWindow.Content = wydarzenieZeSzczegolami;
                mainWindow.MainFrame.NavigationService.Navigate(new WydarzenieZeSzczegolami(wybraneWydarzenie));
                mainWindow.labelStatus.Content = "Jesteś w: Szczegóły wybranego wydarzenia";
              // MiniFrame.NavigationService.Navigate(new WydarzenieZeSzczegolami(wybraneWydarzenie));
            }
        }
        private void WyswietlWydarzenia_doubleclick(object sender, RoutedEventArgs e)
        {
            if(listBox.SelectedItem !=null)
            {
                Button_SzczegolyClick(this, new RoutedEventArgs());
            }
        }

        private void ButtonFiltrujPoLokalizacji_Click(object sender, RoutedEventArgs e)
        {
            var sortedWydarzenia = wydarzenia
            .OrderBy(w => w.MIASTO)
            .ToList();

            listBox.ItemsSource = sortedWydarzenia;

        }

        private void ButtonFiltrujPoCenie_Click(object sender, RoutedEventArgs e)
        {
            var sortedWydarzenia = wydarzenia
            .OrderBy(w => w.Cena_BILETU)
            .ToList();

            listBox.ItemsSource = sortedWydarzenia;

        }
    }
}

