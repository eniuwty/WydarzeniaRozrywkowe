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

namespace WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_Pracownikami
{
    /// <summary>
    /// Logika interakcji dla klasy DodajPracownika.xaml
    /// </summary>
    public partial class DodajPracownika : Page
    {
        public DodajPracownika()
        {
            InitializeComponent();
            TextBoxImie.Focus();
        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        private void textbox_TylkoLitery(object sender, TextCompositionEventArgs e)
        {
            if(!char.IsLetter(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void DodawaniePracownika(object sender, RoutedEventArgs e)
        {
            if(TextBoxImie.Text!="" && TextBoxNazwisko.Text!="")
            {

            string connectionString = "Data Source=wydarzenia.db;Version=3;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            string imie = TextBoxImie.Text;
            string nazwisko = TextBoxNazwisko.Text;
            string insertQuery = $"INSERT INTO Pracownik (Imie, Nazwisko) VALUES ('{imie}', '{nazwisko}');";

            SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection);
            insertCommand.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Dodano do bazy danych nowego pracownika");
            mainWindow.MainFrame.NavigationService.Navigate(new ListaPracowników());
            mainWindow.labelStatus.Content = "Jesteś w: Lista pracowników";
            /*
            TrybAdminaMenu trybAdminaMenu = new TrybAdminaMenu();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = trybAdminaMenu;
            */
            }
            else
            {
                MessageBox.Show("Imie i nazwisko muszą zostać wypełnione");
            }
            
        }

        private void ButtonCofnij(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new ListaPracowników());
            mainWindow.labelStatus.Content = "Jesteś w: Lista pracowników";
            //ListaPracowników listaPracowników = new ListaPracowników();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = listaPracowników;

        }
        private void DodajPracownika_Esc(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Escape) 
            {
                ButtonCofnij(this,new RoutedEventArgs());
            }
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                if(TextBoxImie.IsFocused)
                {
                    TextBoxNazwisko.Focus();
                    TextBoxNazwisko.SelectAll();
                }
                else if(TextBoxNazwisko.IsFocused)
                {
                    DodawaniePracownika(this, new RoutedEventArgs());
                }
            }
        }


    }
}
