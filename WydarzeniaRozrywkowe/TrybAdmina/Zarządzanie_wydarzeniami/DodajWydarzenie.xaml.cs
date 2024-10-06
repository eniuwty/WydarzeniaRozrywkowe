using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_Pracownikami;

namespace WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_wydarzeniami
{
    /// <summary>
    /// Logika interakcji dla klasy DodajWydarzenie.xaml
    /// </summary>
    public partial class DodajWydarzenie : Page
    {
        public DodajWydarzenie(string nazwa_="", string opis_="", int maks_osob_=0, double cena_biletu_ = 0)
        {
            InitializeComponent();
            NazwaTextBox.Text = nazwa_;
            OpisTextBox.Text = opis_;
            if( maks_osob_ > 0 )
            MaksIloscTextBox.Text =(maks_osob_).ToString();
            if( cena_biletu_ > 0 )
            CenaBiletuTextBox.Text=(cena_biletu_).ToString();
            NazwaTextBox.Focus();
            NazwaTextBox.SelectAll();
        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;


        private void ButtonDalejClick(object sender, RoutedEventArgs e)
        {
            if (NazwaTextBox.Text != "" && MaksIloscTextBox.Text != "" && CenaBiletuTextBox.Text != "")
            {
                // Check if MaksIloscTextBox contains only digits
                if (int.TryParse(MaksIloscTextBox.Text, out int maks_osob) && double.TryParse(CenaBiletuTextBox.Text, out double cena_biletu))
                {
                    string nazwa = NazwaTextBox.Text;
                    string opis = OpisTextBox.Text;
                    mainWindow.MainFrame.NavigationService.Navigate(new DodajWydarzenieLokalizacja(nazwa, opis, maks_osob, cena_biletu));
                    mainWindow.labelStatus.Content = "Jesteś w: dodawanie nowego wydarzenia(2/3)";
                    //DodajWydarzenieLokalizacja dodajWydarzenieLokalizacja = new DodajWydarzenieLokalizacja(nazwa, opis, maks_osob, cena_biletu);
                    //Window parentWindow = Window.GetWindow(this);
                    //parentWindow.Content = dodajWydarzenieLokalizacja;
                }
                else
                {
                    MessageBox.Show("Pole 'Maks. ilość osób' musi zawierać liczbę całkowitą, a pole 'Cena biletu' dowolna liczbę.");
                }
            }
            else
            {
                MessageBox.Show("Żadne z pól oznaczone * nie może być puste.");
            }
        }


        private void ButtonCofnij(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzeniaAdmin());
            mainWindow.labelStatus.Content = "Jesteś w: Wyświetlanie wydarzeń";
            //WyswietlWydarzeniaAdmin wyswietlWydarzeniaAdmin = new WyswietlWydarzeniaAdmin();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = wyswietlWydarzeniaAdmin;
        }
        private void DodajWydarzenie_Esc(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Escape)
            {
                ButtonCofnij(this, new RoutedEventArgs());
            }
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (NazwaTextBox.IsKeyboardFocused && NazwaTextBox.Text !="")
                { 
                    OpisTextBox.Focus();
                    OpisTextBox.SelectAll();
                }
                else if (OpisTextBox.IsKeyboardFocused)
                {
                    MaksIloscTextBox.Focus();
                    MaksIloscTextBox.SelectAll();
                }
                else if(MaksIloscTextBox.IsKeyboardFocused && MaksIloscTextBox.Text !="")
                {
                    CenaBiletuTextBox.Focus();
                    CenaBiletuTextBox.SelectAll();
                }
                else if(NazwaTextBox.Text !="" && MaksIloscTextBox.Text != "" && CenaBiletuTextBox.Text !="")
                {
                    ButtonDalejClick(this, new RoutedEventArgs());
                }

            }
        }
    }
}
