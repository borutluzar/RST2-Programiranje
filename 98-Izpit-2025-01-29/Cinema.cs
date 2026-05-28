using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2025_01_29
{
    public class Cinema
    {
        public Cinema() { }

        public Cinema(List<Movie> lstCinemaMovies)
        {
            lstMovies = new List<Movie>(lstCinemaMovies);
        }

        public string City { get; set; }

        private List<Movie> lstMovies = new List<Movie>();
        public List<Movie> Movies
        {
            get
            {
                return lstMovies;
            }
        }

        public List<Hall> Halls { get; } = new List<Hall>();

        public void EmptyHalls(DateTime time, double interval)
        {
            this.Halls.ForEach(hall => 
            { 
                if(hall.Schedule.IsEmpty(time, interval))
                {
                    Console.WriteLine($"V {this.City} je {hall.HallName} dvorana prosta");
                }
            });
        }

        public void PlayMovies(DateTime dtStart)
        {
            Timer timer = new Timer(
                    state =>
                    {
                        if(DateTime.Now.Hour == dtStart.Hour)
                        {
                            foreach (var hall in this.Halls)
                            {
                                Task.Run(() => hall.PlayMovie(dtStart));
                            }
                        }
                    },
                    null,
                    1000,
                    10_000
                );
        }
    }
}
