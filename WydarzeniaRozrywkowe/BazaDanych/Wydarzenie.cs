using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WydarzeniaRozrywkowe.BazaDanych
{
    public class Wydarzenie
    {
        public int Id_Wydarzenia { get; set; }
        public string Nazwa { get; set; }
        public int Osoby_MAKS { get; set; }
        public double Cena_BILETU {  get; set; }
        public string OPIS {  get; set; }
        public string DATA { get; set; }
        public string GODZINA { get; set; }
        public string MIASTO { get; set; }
        public string ADRES { get; set; }
        public string KOD_POCZTOWY { get; set; }
        public string NAZWA_OBIEKTU { get; set; }

        public Wydarzenie(int id_Wydarzenia, string nazwa, int osoby_MAKS, double cena_BILETU, string oPIS, string dATA, string gODZINA, string mIASTO, string aDRES, string kOD_POCZTOWY, string nAZWA_OBIEKTU)
        {
            Id_Wydarzenia = id_Wydarzenia;
            Nazwa = nazwa;
            Osoby_MAKS = osoby_MAKS;
            Cena_BILETU = cena_BILETU;
            OPIS = oPIS;
            DATA = dATA;
            GODZINA = gODZINA;
            MIASTO = mIASTO;
            ADRES = aDRES;
            KOD_POCZTOWY = kOD_POCZTOWY;
            NAZWA_OBIEKTU = nAZWA_OBIEKTU;
        }
    }
}
