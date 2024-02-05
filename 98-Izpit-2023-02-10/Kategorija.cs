using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2023_02_10
{
    public enum KategorijaEnum
    {
        Člani = 0,
        Mladinci = 1,
        Kadeti = 2,
        Cicibani = 3
    }

    public class Kategorija
    {
        public Kategorija(KategorijaEnum vrsta, Spol spol)
        {
            this.Vrsta = vrsta;
            this.Spol = spol;

            this.Ekipe = new();
            this.Discipline = new();
        }

        public KategorijaEnum Vrsta { get; }
        public Spol Spol { get; }

        public List<Ekipa> Ekipe { get; set; }
        public List<Disciplina> Discipline { get; set; }

        public List<Ekipa> Razvrsti()
        {
            return this.Ekipe.OrderBy(ekipa => ekipa.Discipline.Sum(disciplina => disciplina.SkupneTocke)).ToList();

            /*
            // Alternativna rešitev
            SortedList<Ekipa, int> lstEkipe = new();
            foreach(var ekipa in this.Ekipe)
            {
                int skupaj = 0;
                foreach(var disc in ekipa.Discipline)
                {
                    skupaj += disc.SkupneTocke;
                }
                lstEkipe.Add(ekipa, skupaj);
            }
            return lstEkipe;
            */
        }
    }
}
