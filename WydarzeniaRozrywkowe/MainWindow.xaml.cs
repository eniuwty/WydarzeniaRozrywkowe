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
using WydarzeniaRozrywkowe.TrybAdmina;
using WydarzeniaRozrywkowe.TrybAdmina.Zarządzanie_wydarzeniami;
using WydarzeniaRozrywkowe.TrybUżytkownika;
using WydarzeniaRozrywkowe.Nawigacja;

namespace WydarzeniaRozrywkowe
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
        public static class Esc
        {
            public static int myEsc { get; set; }
            public static int logowanie { get; set; }
            public static int Ludziska { get; set; }
            
        }
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            //ButtonAdmin.Focus();
            Esc.myEsc = 1;
            Esc.logowanie = 0;
            Esc.Ludziska = -1;
            //MainFrame.NavigationService.Navigate(new WyswietlWydarzenia());
            NavigationFrame.NavigationService.Navigate(new NawigacjaUzytkownik());
            MainFrame.NavigationService.Navigate(new glowna());
        }

        /*
        private void ButtonUzytkownik_Click(object sender, RoutedEventArgs e)
        {
            WyswietlWydarzenia wyswietlWydarzenia = new WyswietlWydarzenia();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Content = wyswietlWydarzenia;
        }
        private void ButtonAdmin_Click(object sender, RoutedEventArgs e)
        {
            logowanie logowanie = new logowanie();
            logowanie.ShowDialog();

            if(Esc.logowanie==1)
            {

            TrybAdminaMain trybAdminaMain = new TrybAdminaMain();
            trybAdminaMain.Show();
            this.Close();
            }
            
        }
        */
        private void MainWindow_Esc(object sender,  System.Windows.Input.KeyEventArgs e)
        {
            /*
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                if(Esc.myEsc >0) 
                {
                    this.Close();
                }
                else
                {
                Esc.myEsc++;          

                }
            }
            */

        }
        private void Frame_block(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }

        private void ButtonWyswietlWydarzeniaUzytkownik_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new WyswietlWydarzenia());
        }
    }
}
