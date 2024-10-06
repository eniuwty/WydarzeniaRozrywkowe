using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WydarzeniaRozrywkowe.TrybUżytkownika;
using WydarzeniaRozrywkowe.Nawigacja;

namespace WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_wydarzeniami
{
    /// <summary>
    /// Logika interakcji dla klasy WyswietlWydarzeniaAdmin.xaml
    /// </summary>
    public partial class WyswietlWydarzeniaAdmin : Page
    {
        private ObservableCollection<Wydarzenie> wydarzenia = new ObservableCollection<Wydarzenie>();
        public WyswietlWydarzeniaAdmin()
        {
            InitializeComponent();
            WczytajWydarzenia();
            ButtonPoworotDoMenuAdmina.Focus();

        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
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
                            var id = reader["Id_Wydarzenia"];
                            var nazwa = reader["Nazwa"];
                            var Osoby_Maks = reader["Osoby_MAKS"];
                            var Cena_BILETU = reader["Cena_BILETU"];
                            var OPIS = reader["OPIS"];
                            var DATA = reader["DATA"];
                            var GODZINA = reader["GODZINA"];
                            var MIASTO = reader["MIASTO"];
                            var ADRES = reader["ADRES"];
                            var KOD_POCZTOWY = reader["KOD_POCZTOWY"];
                            var NAZWA_OBIEKTU = reader["NAZWA_OBIEKTU"];

                            //////na stringi i inti

                            int id_int = Convert.ToInt32(id);
                            string nazwas = nazwa.ToString();
                            int Osoby_Maks_int = Convert.ToInt32(Osoby_Maks);
                            double Cena_BILETU_double = Convert.ToDouble(Cena_BILETU);
                            string OPISS = OPIS.ToString();
                            string DATAS = DATA.ToString();
                            string GODZINAS = GODZINA.ToString();
                            string MIASTOS = MIASTO.ToString();
                            string ADRESS = ADRES.ToString();
                            string KOD_POCZTOWYS = KOD_POCZTOWY.ToString();
                            string NAZWA_OBIEKTUS = NAZWA_OBIEKTU.ToString();

                            Wydarzenie wydarzenie = new Wydarzenie(id_int, nazwas, Osoby_Maks_int, Cena_BILETU_double, OPISS, DATAS, GODZINAS, MIASTOS, ADRESS, KOD_POCZTOWYS, NAZWA_OBIEKTUS);
                            wydarzenia.Add(wydarzenie);
                        }
                    }

                }
                listBoxWydarzenia.ItemsSource = wydarzenia;
            }
        }

        private void UsunWydarzenie(int idWydarzenia)
        {
            try
            {
                string connectionString = "Data Source=Wydarzenia.db;Version=3;";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand("DELETE FROM Wydarzenie WHERE Id_Wydarzenia = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", idWydarzenia);
                        command.ExecuteNonQuery();
                    }
                }
                // Powiadomienie o sukcesie usuwania (np. MessageBox lub inny komunikat)
                MessageBox.Show("Wydarzenie zostało usunięte z bazy danych.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Obsługa błędów (np. wyświetlenie komunikatu o błędzie)
                MessageBox.Show($"Wystąpił błąd podczas usuwania pracownika: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            //usuwanie z tabeli PracownicyNaWydarzeniu


            try
            {
                string connectionString = "Data Source=Wydarzenia.db;Version=3;";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand("DELETE FROM PracownicyNaWydarzeniu WHERE Id_Wydarzenia = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", idWydarzenia);
                        command.ExecuteNonQuery();
                    }
                }
                // Powiadomienie o sukcesie usuwania (np. MessageBox lub inny komunikat)
               
            }
            catch (Exception ex)
            {
                // Obsługa błędów (np. wyświetlenie komunikatu o błędzie)
                MessageBox.Show($"Wystąpił błąd podczas usuwania w tabeli PracownicyNaWydarzeniu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           

        }

        private void Button_Szczegoly_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxWydarzenia.SelectedItem is Wydarzenie wydarzenie)
            {
                Wydarzenie wybraneWydarzenie = (Wydarzenie)listBoxWydarzenia.SelectedItem;
                mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzenieZeSzczegolamiAdmin(wybraneWydarzenie));
                mainWindow.labelStatus.Content = "Jesteś w: Szczegóły wydarzenia";
                //WyswietlWydarzenieZeSzczegolamiAdmin wydarzenieZeSzczegolami = new WyswietlWydarzenieZeSzczegolamiAdmin(wybraneWydarzenie);
                //Window parentWindow = Window.GetWindow(this);
                //parentWindow.Content = wydarzenieZeSzczegolami;
            }
        }

        private void Button_Usun_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxWydarzenia.SelectedItem is Wydarzenie wydarzenie)
            {
                MessageBoxResult resultt = MessageBox.Show(
            "Czy na pewno chcesz usunąć wydarzenie z bazy danych?",
            "Uwaga",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
                if (resultt == MessageBoxResult.Yes)
                {
                        int idWydarzenia = wydarzenie.Id_Wydarzenia;
                        // Wywołaj metodę usuwającą pracownika z bazy danych na podstawie idPracownika
                        UsunWydarzenie(idWydarzenia);

                        // Odśwież listę pracowników
                        wydarzenia.Clear();

                        WczytajWydarzenia();
                   // mainWindow.labelStatus.Content = "Jesteś w: Wyświetlanie wydarzeń";
                    //mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzeniaAdmin());

                }
                if(resultt == MessageBoxResult.No)
                {

                    //mainWindow.labelStatus.Content = "Jesteś w: Wyświetlanie wydarzeń";
                    //mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzeniaAdmin());
                }

            }
           // mainWindow.labelStatus.Content = "Jesteś w: Wyświetlanie wydarzeń";
           // mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzeniaAdmin());

        }

        private void Button_PowrotDMA_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new glowna_admin());
            mainWindow.labelStatus.Content = "Jesteś w: Strona główna";
            //TrybAdminaMenu trybAdminaMenu = new TrybAdminaMenu();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = trybAdminaMenu;
        }

        private void Button_Dodaj_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new DodajWydarzenie());
            mainWindow.labelStatus.Content = "Jesteś w: Dodawanie wydarzenia(1/3)";
            //DodajWydarzenie dodajWydarzenie = new DodajWydarzenie();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = dodajWydarzenie;
        }

        private void WyswietlWydarzeniaAdmin_Esc(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Escape)
            {
                Button_PowrotDMA_Click(this, new RoutedEventArgs());
            }

            if((e.Key == System.Windows.Input.Key.Back || e.Key == System.Windows.Input.Key.Delete) && listBoxWydarzenia.SelectedItem is Wydarzenie wydarzenie)
            {
                Button_Usun_Click(this,new RoutedEventArgs());

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

            listBoxWydarzenia.ItemsSource = sortedWydarzenia;
        }

        private void ButtonFiltrujPoNazwie_Click(object sender, RoutedEventArgs e)
        {
            var sortedWydarzenia = wydarzenia
                .OrderBy(w => w.Nazwa)
                .ToList();
            
            listBoxWydarzenia.ItemsSource = sortedWydarzenia;
        }
        private void ListBoxWydarzenia_doubleclick(object sender, RoutedEventArgs e)
        {
            if(listBoxWydarzenia.SelectedItem !=null)
            {
                Button_Szczegoly_Click(this, new RoutedEventArgs());
            }
        }

        private void Button_Edytuj_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxWydarzenia.SelectedItem is Wydarzenie wydarzenie)
            {
                mainWindow.MainFrame.NavigationService.Navigate ( new EdytujWydarzenie(
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
                    wydarzenie.NAZWA_OBIEKTU
                ));
                mainWindow.labelStatus.Content = "Jesteś w: Edytowanie wydarzenia";
                //mainWindow.MainFrame.NavigationService.Navigate
               // Window parentWindow = Window.GetWindow(this);
               // parentWindow.Content = edytujWydarzenie;
            }
        }

        private void ButtonFiltrujPoLokalizacji_Click(object sender, RoutedEventArgs e)
        {
            var sortedWydarzenia = wydarzenia
            .OrderBy(w => w.MIASTO)
            .ToList();

            listBoxWydarzenia.ItemsSource = sortedWydarzenia;
        }

        private void ButtonFiltrujPoCenie_Click(object sender, RoutedEventArgs e)
        {
            var sortedWydarzenia = wydarzenia
            .OrderBy(w => w.Cena_BILETU)
            .ToList();

            listBoxWydarzenia.ItemsSource = sortedWydarzenia;
        }
    }
    }
    


