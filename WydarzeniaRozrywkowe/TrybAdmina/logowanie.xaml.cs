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
using System.Windows.Shapes;
using WydarzeniaRozrywkowe.Nawigacja;

namespace WydarzeniaRozrywkowe.TrybAdmina
{
    /// <summary>
    /// Logika interakcji dla klasy logowanie.xaml
    /// </summary>
    public partial class logowanie : Page
    {
        protected string login = "admin";
        protected string password = "admin";

        
        public logowanie()
        {
            InitializeComponent();
            textbox_login.Focus();
            Esc.logowanie = 0;
        }
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private void Button_zaloguj_click(object sender, RoutedEventArgs e)
        {
            if (textbox_login.Text == login && textbox_password.Password == password)
            {
                Esc.logowanie = 1;
                MessageBox.Show("Logowanie powiodło się!");
                mainWindow.labelzalogowano.Content = "Zalogowano jako: Admin";
                mainWindow.labelStatus.Content = "Jesteś w: Strona Główna";
                mainWindow.NavigationFrame.NavigationService.Navigate(new NawigacjaAdmin());
                mainWindow.MainFrame.NavigationService.Navigate(new glowna_admin());

            }
            else
            {
                MessageBox.Show("Błędne hasło lub login");
                textbox_login.Focus();
                textbox_login.SelectAll();
                Esc.logowanie = 0;
            }
        }
        private void logowanie_esc(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                
            }
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                if(textbox_login.IsKeyboardFocused && textbox_login.Text !="")
                {
                    textbox_password.Focus();
                    textbox_password.SelectAll();
                }
                else if(textbox_password.IsKeyboardFocused && textbox_password.Password !="")
                {
                    Button_zaloguj_click(this, new RoutedEventArgs());
                }
                else if( textbox_login.IsKeyboardFocused == false && textbox_password.IsKeyboardFocused == false)
                {
                    Button_zaloguj_click(this, new RoutedEventArgs());
                }


            }
            if(e.Key == System.Windows.Input.Key.Tab)
            {
                if(textbox_login.IsFocused)
                {
                    textbox_password.Focus();
                }
                else
                {
                    textbox_login.Focus();
                }
            }
        }


    }
    
}
