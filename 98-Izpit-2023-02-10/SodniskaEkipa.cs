using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2023_02_10
{
    public class SodniskaEkipa
    {
        public SodniskaEkipa(int stevilkaEkipe)
        {
            this.StevilkaEkipe = stevilkaEkipe;

            this.Sodniki = new List<Sodnik>();                        
        }

        public int StevilkaEkipe { get; }

        public List<Sodnik> Sodniki { get; set; }
    }
}
