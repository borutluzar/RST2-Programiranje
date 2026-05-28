namespace Izpit_2025_01_29
{
    public class Movie
    {
        public string Name { get; set; }

        public Genre Genre { get; set; }

        private double length;
        public double Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }
    }

    public enum Genre
    {
        Horror = 1,
        Comedy = 2,
        Thriller = 3,
        Action = 4
    }
}
