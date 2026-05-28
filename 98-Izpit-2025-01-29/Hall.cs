using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2025_01_29
{
    public class Hall
    {
        public string HallName { get; set; }

        private Schedule lstSchedule = new Schedule();
        public Schedule Schedule
        {
            get
            {
                return lstSchedule;
            }
        }    
        
        public void PlayMovie(DateTime dtStart)
        {
            Console.WriteLine($"{dtStart:dd. MM. yyyy} ob {dtStart:HH:mm} smo začeli s predvajanjem filma");
        }
    }
}
