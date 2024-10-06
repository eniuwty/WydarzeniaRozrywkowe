using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WydarzeniaRozrywkowe.BazaDanych
{
    public class Pracownik
    {
        public int Id_Pracownik { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        public Pracownik(int id_Pracownik, string imie, string nazwisko)
        {
            Id_Pracownik = id_Pracownik;
            Imie = imie;
            Nazwisko = nazwisko;
        }
    }
}

