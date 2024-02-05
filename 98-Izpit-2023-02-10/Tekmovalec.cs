using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2023_02_10
{
    public class Tekmovalec
    {
        public Tekmovalec(int id, string priimek, string ime, string drustvo, int letnicaRojstva) 
        {
            this.ID = id;
            this.Priimek = priimek;
            this.Ime = ime;
            this.Drustvo = drustvo;
            this.LetnicaRojstva = letnicaRojstva;
        }

        public int ID { get; }

        public string Priimek { get; }

        public string Ime { get; }

        public string Drustvo { get; }

        public int LetnicaRojstva { get; }
    }
}
