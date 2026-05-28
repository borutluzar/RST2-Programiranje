using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2025_01_29
{
    interface ISchedule
    {
        public bool IsEmpty(DateTime dt, double interval);
    }
}
