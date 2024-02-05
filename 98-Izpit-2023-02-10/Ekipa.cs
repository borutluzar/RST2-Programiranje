using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2023_02_10
{
    public enum Spol
    {
        Moški = 0,
        Ženske = 1
    }

    public class Ekipa
    {
        public Ekipa(string ime, string drustvo, Spol spol)
        {
            this.Ime = ime;
            this.Drustvo = drustvo;
            this.Spol = spol;

            this.Tekmovalci = new();
            this.Discipline = new();
        }

        public string Ime { get; }
        public string Drustvo { get; }
        public Spol Spol { get; }

        public SodniskaEkipa Sodniki { get; set; }

        public List<Tekmovalec> Tekmovalci { get; set; }
        public List<Disciplina> Discipline { get; set; }
    }
}
