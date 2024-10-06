using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WydarzeniaRozrywkowe.BazaDanych;

namespace WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_wydarzeniami
{
    public partial class EdytujWydarzenie : Page
    {
        int idWydarzenia;
        List<Pracownik> availableEmployees = new List<Pracownik>();
        List<Pracownik> assignedEmployees = new List<Pracownik>();

        public EdytujWydarzenie(int id, string nazwa, string opis, int maksOsob, double cena, string data, string godzina, string miasto, string adres, string kodPocztowy, string nazwaObiektu)
        {
            InitializeComponent();

            idWydarzenia = id;
            NazwaTextBox.Text = nazwa;
            OpisTextBox.Text = opis;
            MaksIloscTextBox.Text = maksOsob.ToString();
            CenaBiletuTextBox.Text = cena.ToString();
            DataTextBox.Text = data;
            GodzinaTextBox.Text = godzina;
            MiastoTextBox.Text = miasto;
            AdresTextBox.Text = adres;
            KodPocztowyTextBox.Text = kodPocztowy;
            NazwaObiektuTextBox.Text = nazwaObiektu;

            LoadEmployees();
            button_anuluj.Focus();


        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;


        private void LoadEmployees()
        {
            string connectionString = "Data Source=wydarzenia.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Load all employees
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Pracownik", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Log all column names and values for debugging
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                string columnValue = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString();
                                Console.WriteLine($"{columnName}: {columnValue}");
                            }

                            var pracownik = new Pracownik(
                                Convert.ToInt32(reader["Id_Pracownik"]),
                                reader["Imie"].ToString(),
                                reader["Nazwisko"].ToString()
                            );
                            availableEmployees.Add(pracownik);
                        }
                    }
                }

                // Load assigned employees
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM PracownicyNaWydarzeniu WHERE Id_Wydarzenia = @idWydarzenia", connection))
                {
                    command.Parameters.AddWithValue("@idWydarzenia", idWydarzenia);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Log all column names and values for debugging
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                string columnValue = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString();
                                Console.WriteLine($"{columnName}: {columnValue}");
                            }

                            int pracownikId = Convert.ToInt32(reader["Id_Pracownika"]);
                            var pracownik = availableEmployees.FirstOrDefault(p => p.Id_Pracownik == pracownikId);
                            if (pracownik != null)
                            {
                                assignedEmployees.Add(pracownik);
                                availableEmployees.Remove(pracownik);
                            }
                        }
                    }
                }

                AvailableEmployeesListBox.ItemsSource = availableEmployees;
                AssignedEmployeesListBox.ItemsSource = assignedEmployees;
            }
        }



        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableEmployeesListBox.SelectedItem is Pracownik selectedEmployee)
            {
                availableEmployees.Remove(selectedEmployee);
                assignedEmployees.Add(selectedEmployee);
                RefreshEmployeeLists();
            }
        }

        private void RemoveEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (AssignedEmployeesListBox.SelectedItem is Pracownik selectedEmployee)
            {
                assignedEmployees.Remove(selectedEmployee);
                availableEmployees.Add(selectedEmployee);
                RefreshEmployeeLists();
            }
        }

        private void RefreshEmployeeLists()
        {
            AvailableEmployeesListBox.ItemsSource = null;
            AvailableEmployeesListBox.ItemsSource = availableEmployees;
            AssignedEmployeesListBox.ItemsSource = null;
            AssignedEmployeesListBox.ItemsSource = assignedEmployees;
        }

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

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            string nazwa = NazwaTextBox.Text;
            string opis = OpisTextBox.Text;
            int maksOsob;
            double cena;

            // Validate maksOsob (maximum number of attendees)
            if (!int.TryParse(MaksIloscTextBox.Text, out maksOsob))
            {
                MessageBox.Show("Niepoprawna wartość dla Maks. Ilość osób. Wprowadź liczbę całkowitą.");
                return;
            }

            // Validate cena (ticket price)
            if (!double.TryParse(CenaBiletuTextBox.Text, out cena))
            {
                MessageBox.Show("Niepoprawna wartość dla Ceny biletu. Wprowadź liczbę zmiennoprzecinkową.");
                return;
            }

            // Validate if any field is empty
            if (string.IsNullOrEmpty(nazwa) || string.IsNullOrEmpty(opis) || string.IsNullOrEmpty(DataTextBox.Text) ||
                string.IsNullOrEmpty(GodzinaTextBox.Text) || string.IsNullOrEmpty(MiastoTextBox.Text) ||
                string.IsNullOrEmpty(AdresTextBox.Text) || string.IsNullOrEmpty(KodPocztowyTextBox.Text) ||
                string.IsNullOrEmpty(NazwaObiektuTextBox.Text))
            {
                MessageBox.Show("Żadne z pól nie może być puste.");
                return;
            }

            // Validate date format
            if (!ValidateDateFormat(DataTextBox.Text))
            {
                MessageBox.Show("Niepoprawny format daty. Poprawny format to DD.MM.RRRR.");
                DataTextBox.Focus();
                DataTextBox.SelectAll();
                return;
            }

            // Proceed with update
            string data = DataTextBox.Text;
            string godzina = GodzinaTextBox.Text;
            string miasto = MiastoTextBox.Text;
            string adres = AdresTextBox.Text;
            string kodPocztowy = KodPocztowyTextBox.Text;
            string nazwaObiektu = NazwaObiektuTextBox.Text;

            string connectionString = "Data Source=wydarzenia.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string updateQuery = $"UPDATE Wydarzenie SET Nazwa='{nazwa}', Osoby_MAKS={maksOsob}, Cena_BILETU={cena}, OPIS='{opis}', DATA='{data}', GODZINA='{godzina}', MIASTO='{miasto}', ADRES='{adres}', KOD_POCZTOWY='{kodPocztowy}', NAZWA_OBIEKTU='{nazwaObiektu}' WHERE Id_Wydarzenia={idWydarzenia}";
                SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection);
                updateCommand.ExecuteNonQuery();

                // Update employees
                SQLiteCommand deleteCommand = new SQLiteCommand("DELETE FROM PracownicyNaWydarzeniu WHERE Id_Wydarzenia=@idWydarzenia", connection);
                deleteCommand.Parameters.AddWithValue("@idWydarzenia", idWydarzenia);
                deleteCommand.ExecuteNonQuery();

                foreach (var pracownik in assignedEmployees)
                {
                    SQLiteCommand insertCommand = new SQLiteCommand("INSERT INTO PracownicyNaWydarzeniu (Id_Wydarzenia, Id_Pracownika) VALUES (@idWydarzenia, @idPracownika)", connection);
                    insertCommand.Parameters.AddWithValue("@idWydarzenia", idWydarzenia);
                    insertCommand.Parameters.AddWithValue("@idPracownika", pracownik.Id_Pracownik);
                    insertCommand.ExecuteNonQuery();
                }
                connection.Close();
            }

            MessageBox.Show("Wydarzenie zostało zaktualizowane.");
            Anuluj_Click(this, new RoutedEventArgs());

            //WyswietlWydarzeniaAdmin wyswietlWydarzeniaAdmin = new WyswietlWydarzeniaAdmin();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = wyswietlWydarzeniaAdmin;
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzeniaAdmin());
            mainWindow.labelStatus.Content = "Jesteś w: Wyświetlanie wydarzeń";
            //WyswietlWydarzeniaAdmin wyswietlWydarzeniaAdmin = new WyswietlWydarzeniaAdmin();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = wyswietlWydarzeniaAdmin;
        }
        private void Cofnij_esc(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
             {
                        Anuluj_Click(this,new RoutedEventArgs());
             }
        }

        private void MouseDoubleCLick_avalibleEmployees(object sender, MouseButtonEventArgs e)
        {
            if(AvailableEmployeesListBox.SelectedItem !=null)
            {
                AddEmployee_Click(this, new RoutedEventArgs());
            }

        }

        private void MouseDoubleClick_assignedEmployees(object sender, MouseButtonEventArgs e)
        {
            if(AssignedEmployeesListBox.SelectedItem != null)
            {
                RemoveEmployee_Click(this, new RoutedEventArgs());
            }
        }
    }
}
