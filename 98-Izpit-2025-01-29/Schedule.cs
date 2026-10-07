using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2025_01_29
{
    public class Schedule : ISchedule, IEnumerable<(DateTime,Movie)>
    {
        private List<(DateTime StartTime, Movie Movie)> lstSchedule = new List<(DateTime, Movie)>();

        public (DateTime, Movie) this[int i]
        {
            get
            {
                return lstSchedule[i];
            }
            set
            {
                lstSchedule[i] = value;
            }
        }
        public void Add(DateTime date, Movie movie)
        {
            lstSchedule.Add((date, movie));
        }

        public IEnumerator<(DateTime, Movie)> GetEnumerator()
        {
            return lstSchedule.GetEnumerator();
        }

        /// <summary>
        /// Checks whether the schedule is empty in the given time interval.
        /// </summary>
        /// <param name="dt">Start time</param>
        /// <param name="interval">In minutes</param>
        /// <returns>True if yes, false otherwise.</returns>
        public bool IsEmpty(DateTime dt, double interval)
        {
            DateTime dtStart = dt;
            DateTime dtEnd = dt.AddMinutes(interval);

            bool isEmpty = true;
            foreach (var pair in lstSchedule)
            {
                DateTime dtEndMovie = pair.StartTime.AddMinutes(pair.Movie.Length);
                if (
                    (pair.StartTime > dtStart && pair.StartTime < dtEnd)
                    ||
                    (dtEndMovie > dtStart && dtEndMovie < dtEnd)
                   )
                {
                    isEmpty = false;
                    break;
                }
            }
            return isEmpty;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return lstSchedule.GetEnumerator();
        }
    }
}
