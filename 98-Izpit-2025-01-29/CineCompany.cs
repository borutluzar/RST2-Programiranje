using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2025_01_29
{
    public class CineCompany
    {
        public string Name { get; set; }

        public string Address { get; set; }

        private List<Cinema> lstCinemas = new List<Cinema>();
        public List<Cinema> Cinemas
        {
            get
            {
                return lstCinemas;
            }            
        }

        // Izberemo slovar, ker na podlagi ključa (email) v O(1) lahko pridobimo ustreznega uporabnika.
        public Dictionary<string, Member> Members = new Dictionary<string, Member>();
    }
}
