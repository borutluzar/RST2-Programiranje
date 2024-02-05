using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2023_02_10
{
    public class Tekmovanje
    {
        public Tekmovanje(string ime, string kraj, int leto)
        {
            this.Ime = ime;
            this.Kraj = kraj;
            this.Leto = leto;

            this.Kategorije = new();
        }

        public string Ime { get; }
        public string Kraj { get; }
        public int Leto { get; }

        public List<Kategorija> Kategorije { get; set; }

        public List<Ekipa> VrstniRed(KategorijaEnum kategorija, Spol spol)
        {
            return Kategorije.Single(x => x.Vrsta == kategorija && x.Spol == spol).Razvrsti();
        }
    }
}
