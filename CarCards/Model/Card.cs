using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCards.Model
{
    public class Card
    {
        //id
        public int Id { get; set; }

        //random stuff
        public string Marka { get; set; }
        public string Tipus { get; set; }
        public string Orszag { get; set; }
        public string Uzemanyag { get; set; }
        public int Evjarat { get; set; }

        //car datas
        public int Vegsebesseg { get; set; }
        public float Gyorsulas { get; set; }
        public int Suly { get; set; }
        public int Terfogat { get; set; }
        public int Loero { get; set; }
        public int Nyomatek { get; set; }
        public string Kep_utvonala { get; set; }

    }
}
