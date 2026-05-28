namespace Izpit_2025_01_29
{
    public class Program
    {
        static void Main(string[] args)
        {
            CineCompany company = new CineCompany();

            Hall hallNM1 = new Hall() { HallName = "Modra" };
            Hall hallNM2 = new Hall() { HallName = "Zelena" };
            Hall hallNM3 = new Hall() { HallName = "Bela" };

            Hall hallCE1 = new Hall() { HallName = "Rumena" };
            Hall hallCE2 = new Hall() { HallName = "Oranžna" };
            Hall hallCE3 = new Hall() { HallName = "Črna" };

            Movie movie1 = new Movie() { Name = "Zootopia", Length = 90 };
            Movie movie2 = new Movie() { Name = "Nosferatu", Length = 120 };
            Movie movie3 = new Movie() { Name = "Batman", Length = 65 };
            Movie movie4 = new Movie() { Name = "Titanic", Length = 180 };

            Cinema cineNM = new Cinema(new List<Movie>() { movie1, movie2, movie3 }) { City = "Novo mesto" };
            cineNM.Halls.Add(hallNM1);
            cineNM.Halls.Add(hallNM2);
            cineNM.Halls.Add(hallNM3);

            Cinema cineCE = new Cinema(new List<Movie>() { movie1, movie3, movie4 }) { City = "Celje" };
            cineCE.Halls.Add(hallCE1);
            cineCE.Halls.Add(hallCE2);
            cineCE.Halls.Add(hallCE3);

            hallNM1.Schedule.Add(new DateTime(2026, 1, 14, 17, 0, 0), movie1);
            hallNM1.Schedule.Add(new DateTime(2026, 1, 14, 19, 0, 0), movie2);
            hallNM1.Schedule.Add(new DateTime(2026, 1, 14, 22, 0, 0), movie3);

            hallNM2.Schedule.Add(new DateTime(2026, 1, 14, 16, 40, 0), movie2);

            Console.WriteLine("Prazne dvorane: ");
            cineNM.EmptyHalls(new DateTime(2026, 1, 14, 16, 0, 0), 180);

            DateTime dtDisplay = new DateTime(2026, 1, 14, 17, 5, 0);
            Console.WriteLine($"Število filmov, ki se predvajajo v času {dtDisplay:HH:mm, dd. MM. yyyy}: " +
                $"{cineNM.DisplayedMovies(dtDisplay)}");

            Console.WriteLine("Začnimo s predvajanjem filmov: ");
            DateTime dtDisplay2 = DateTime.Now;
            cineNM.PlayMovies(dtDisplay2);

            Console.ReadLine();
        }
    }

    public static class Extensions
    {
        public static int DisplayedMovies(this Cinema cinema, DateTime time)
        {
            HashSet<Movie> hshMovie = new HashSet<Movie>();
            foreach(var hall in cinema.Halls)
            {
                foreach(var pair in hall.Schedule)
                {
                    if(pair.Item1 <= time && pair.Item1.AddMinutes(pair.Item2.Length) >= time)
                    {
                        hshMovie.Add(pair.Item2);
                    }
                }
            }
            return hshMovie.Count;
        }
    }
}
