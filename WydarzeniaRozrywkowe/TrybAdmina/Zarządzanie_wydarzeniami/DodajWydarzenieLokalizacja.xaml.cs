using System;
using System.Data.SQLite;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_wydarzeniami
{
    public partial class DodajWydarzenieLokalizacja : Page
    {
        private string nazwa;
        private string opis;
        private int maks_osob;
        private double cena;
        private int lastInsertId; // Pole przechowujące ID ostatnio dodanego wydarzenia

        public DodajWydarzenieLokalizacja(string nazwa, string opis, int maks_osob, double cena)
        {
            InitializeComponent();
            this.nazwa = nazwa;
            this.opis = opis;
            this.maks_osob = maks_osob;
            this.cena = cena;
            DataTextBox.Focus();

            // Dodaj obsługę zdarzenia PreviewTextInput do GodzinaTextBox
            GodzinaTextBox.PreviewTextInput += GodzinaTextBox_PreviewTextInput;
        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        // Metoda do walidacji formatu daty
        private bool ValidateDateFormat(string date)
        {
            // Sprawdź czy data ma format DD.MM.RRRR
            if (!Regex.IsMatch(date, @"^\d{2}\.\d{2}\.\d{4}$"))
                return false;

            // Spróbuj sparsować do DateTime
            if (!DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                return false;

            return true;
        }

        // Metoda do walidacji formatu godziny
        private bool ValidateTimeFormat(string time)
        {
            // Sprawdź czy czas ma format HH:mm
            if (!Regex.IsMatch(time, @"^(0[0-9]|1[0-9]|2[0-3]):([0-5][0-9])$"))
                return false;

            // Spróbuj sparsować do TimeSpan
            if (!TimeSpan.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, out _))
                return false;

            return true;
        }

        // Obsługa zdarzenia LostFocus dla pola DataTextBox
        private void DataTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.Trim(); // Usuń białe znaki z początku i końca

            if (!ValidateDateFormat(textBox.Text))
            {
                MessageBox.Show("Niepoprawny format daty. Poprawny format to DD.MM.RRRR.");
                textBox.Focus();
                textBox.SelectAll();
            }
        }

        // Obsługa zdarzenia LostFocus dla pola GodzinaTextBox
        private void GodzinaTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.Trim(); // Usuń białe znaki z początku i końca

            if (!ValidateTimeFormat(textBox.Text))
            {
                MessageBox.Show("Niepoprawny format godziny. Poprawny format to HH:MM.");
                textBox.Focus();
                textBox.SelectAll();
            }
        }

        // Metoda do obsługi zdarzenia kliknięcia przycisku Dalej
        private void DalejLokClick(object sender, RoutedEventArgs e)
        {
            if (ValidateInputFields())
            {
                string data = DataTextBox.Text;
                string godzina = GodzinaTextBox.Text;
                string miasto = MiastoTextBox.Text;
                string adres = AdresTextBox.Text;
                string kod_pocztowy = Kod_Pocztowy_TextBox.Text;
                string nazwa_obiektu = NazwaObiektu_TextBox.Text;

                // Dodawanie wydarzenia do bazy danych
                string connectionString = "Data Source=wydarzenia.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Wydarzenie (Nazwa, Osoby_Maks, Cena_BILETU, OPIS, DATA, GODZINA, MIASTO, ADRES, KOD_POCZTOWY, NAZWA_OBIEKTU) " +
                                         "VALUES (@nazwa, @maks_osob, @cena, @opis, @data, @godzina, @miasto, @adres, @kod_pocztowy, @nazwa_obiektu);";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@nazwa", nazwa);
                        insertCommand.Parameters.AddWithValue("@maks_osob", maks_osob);
                        insertCommand.Parameters.AddWithValue("@cena", cena);
                        insertCommand.Parameters.AddWithValue("@opis", opis);
                        insertCommand.Parameters.AddWithValue("@data", data);
                        insertCommand.Parameters.AddWithValue("@godzina", godzina);
                        insertCommand.Parameters.AddWithValue("@miasto", miasto);
                        insertCommand.Parameters.AddWithValue("@adres", adres);
                        insertCommand.Parameters.AddWithValue("@kod_pocztowy", kod_pocztowy);
                        insertCommand.Parameters.AddWithValue("@nazwa_obiektu", nazwa_obiektu);

                        insertCommand.ExecuteNonQuery();

                        // Pobierz ID ostatnio dodanego wydarzenia
                        string lastInsertIdQuery = "SELECT last_insert_rowid()";
                        using (SQLiteCommand lastInsertIdCommand = new SQLiteCommand(lastInsertIdQuery, connection))
                        {
                            lastInsertId = Convert.ToInt32(lastInsertIdCommand.ExecuteScalar());
                            MessageBox.Show($"Dodano Wydarzenie. Nowo dodany rekord ma Id_Wydarzenia: {lastInsertId}");
                        }
                    }
                }

                // Przejdź do strony dodawania pracowników do wydarzenia
                mainWindow.MainFrame.NavigationService.Navigate(new DodawaniePracownikowDoWydarzenia(lastInsertId));
                mainWindow.labelStatus.Content = "Jesteś w: Dodawanie nowego wydarzenia(3/3)";
            }
        }

        // Metoda do walidacji wszystkich pól wejściowych
        private bool ValidateInputFields()
        {
            if (string.IsNullOrEmpty(NazwaObiektu_TextBox.Text) || string.IsNullOrEmpty(DataTextBox.Text) ||
                string.IsNullOrEmpty(MiastoTextBox.Text) || string.IsNullOrEmpty(AdresTextBox.Text) ||
                string.IsNullOrEmpty(Kod_Pocztowy_TextBox.Text))
            {
                MessageBox.Show("Żadne z pól nie może być puste.");
                return false;
            }

            if (!ValidateDateFormat(DataTextBox.Text))
            {
                MessageBox.Show("Niepoprawny format daty. Poprawny format to DD.MM.RRRR.");
                DataTextBox.Focus();
                DataTextBox.SelectAll();
                return false;
            }

            return true;
        }


        // Obsługa zdarzenia kliknięcia przycisku Cofnij
        private void ButtonCofnij(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new DodajWydarzenie(nazwa, opis, maks_osob, cena));
            mainWindow.labelStatus.Content = "Jesteś w: Dodawanie nowego wydarzenia(1/3)";
           //DodajWydarzenie dodajWydarzenie = new DodajWydarzenie(nazwa, opis, maks_osob, cena);
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = dodajWydarzenie;
        }

        // Obsługa zdarzenia klawiszy Escape i Enter
        private void DodajWydarzenieLokalizacja_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ButtonCofnij(this, new RoutedEventArgs());
            }
            else if (e.Key == Key.Enter)
            {
                if (DataTextBox.IsKeyboardFocused && !string.IsNullOrEmpty(DataTextBox.Text))
                {
                    GodzinaTextBox.Focus();
                    GodzinaTextBox.SelectAll();
                }
                else if (GodzinaTextBox.IsKeyboardFocused && !string.IsNullOrEmpty(GodzinaTextBox.Text))
                {
                    MiastoTextBox.Focus();
                    MiastoTextBox.SelectAll();
                }
                else if (MiastoTextBox.IsKeyboardFocused && !string.IsNullOrEmpty(MiastoTextBox.Text))
                {
                    AdresTextBox.Focus();
                    AdresTextBox.SelectAll();
                }
                else if (AdresTextBox.IsKeyboardFocused && !string.IsNullOrEmpty(AdresTextBox.Text))
                {
                    Kod_Pocztowy_TextBox.Focus();
                    Kod_Pocztowy_TextBox.SelectAll();
                }
                else if (Kod_Pocztowy_TextBox.IsKeyboardFocused && !string.IsNullOrEmpty(Kod_Pocztowy_TextBox.Text))
                {
                    NazwaObiektu_TextBox.Focus();
                    NazwaObiektu_TextBox.SelectAll();
                }
                else if (ValidateInputFields())
                {
                    DalejLokClick(this, new RoutedEventArgs());
                }
            }
        }

        // Obsługa zdarzenia PreviewTextInput dla GodzinaTextBox
        private void GodzinaTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9:]"); // Dozwolone tylko cyfry i dwukropek
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DodajWydarzenieLokalizacja_Esc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ButtonCofnij(this, new RoutedEventArgs());
            }
            else if (e.Key == Key.Enter)
            {
                if (DataTextBox.IsKeyboardFocused && !string.IsNullOrEmpty(DataTextBox.Text))
                {
                    GodzinaTextBox.Focus();
                    GodzinaTextBox.SelectAll();
                }
                else if (GodzinaTextBox.IsKeyboardFocused && !string.IsNullOrEmpty(GodzinaTextBox.Text))
                {
                    MiastoTextBox.Focus();
                    MiastoTextBox.SelectAll();
                }
                else if (MiastoTextBox.IsKeyboardFocused && !string.IsNullOrEmpty(MiastoTextBox.Text))
                {
                    AdresTextBox.Focus();
                    AdresTextBox.SelectAll();
                }
                else if (AdresTextBox.IsKeyboardFocused && !string.IsNullOrEmpty(AdresTextBox.Text))
                {
                    Kod_Pocztowy_TextBox.Focus();
                    Kod_Pocztowy_TextBox.SelectAll();
                }
                else if (Kod_Pocztowy_TextBox.IsKeyboardFocused && !string.IsNullOrEmpty(Kod_Pocztowy_TextBox.Text))
                {
                    NazwaObiektu_TextBox.Focus();
                    NazwaObiektu_TextBox.SelectAll();
                }
                else if (ValidateInputFields())
                {
                    DalejLokClick(this, new RoutedEventArgs());
                }
            }
        }
    }
}
