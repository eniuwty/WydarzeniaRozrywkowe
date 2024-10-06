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
using WydarzeniaRozrywkowe.BazaDanych;

namespace WydarzeniaRozrywkowe.TrybUżytkownika
{
    /// <summary>
    /// Logika interakcji dla klasy WydarzenieZeSzczegolami.xaml
    /// </summary>
    public partial class WydarzenieZeSzczegolami : Page
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        Wydarzenie wydarzenie;
        public WydarzenieZeSzczegolami(Wydarzenie wydarzenie)
        {
            InitializeComponent();
            this.wydarzenie = wydarzenie;
            Uzupelnij();
            Button_Cofnij.Focus();
        }

        public void Uzupelnij()
        {
            NazwaTextBlock.Text = wydarzenie.Nazwa;
            OsobyMaksTextBlock.Text = wydarzenie.Osoby_MAKS.ToString();
            CenaBiletuTextBlock.Text = wydarzenie.Cena_BILETU.ToString();
            OpisTextBlock.Text = wydarzenie.OPIS;
            DataTextBlock.Text = wydarzenie.DATA;
            GodzinaTextBlock.Text = wydarzenie.GODZINA;
            MiastoTextBlock.Text = wydarzenie.MIASTO;
            AdresTextBlock.Text = wydarzenie.ADRES;
            KodPocztowyTextBlock.Text = wydarzenie.KOD_POCZTOWY;
            NazwaObiektuTextBlock.Text = wydarzenie.NAZWA_OBIEKTU;

        }

        private void Button_Cofnij_Click(object sender, RoutedEventArgs e)
        {
            //WyswietlWydarzenia wyswietlWydarzenia = new WyswietlWydarzenia();
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Content = wyswietlWydarzenia;
            mainWindow.MainFrame.NavigationService.Navigate(new WyswietlWydarzenia());
        }
        private void WydarzenieZeSzczegolami_Esc(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Escape)
            {
                Button_Cofnij_Click(this, new RoutedEventArgs());
                Esc.myEsc = 0;
            }

        }
    }
}
