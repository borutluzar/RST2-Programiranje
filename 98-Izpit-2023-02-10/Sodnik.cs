using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2023_02_10
{
    public class Sodnik
    {
        public Sodnik(string priimek, string ime)
        {
            this.Priimek = priimek;
            this.Ime = ime;            
        }

        public string Ime { get; }
        public string Priimek { get; }
    }
}
