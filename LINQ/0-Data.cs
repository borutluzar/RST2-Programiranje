using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace LINQ
{
    /// <summary>
    /// Arh, Q49
    /// LINQ - Language INtegrated Query
    /// Je posebna oblika pisanja poizvedb v podatkovnih strukturah.
    /// Uporabljamo ga lahko za poizvedbe po .Net zbirkah, relacijskih podatkovnih bazah ali pa po XML dokumentih.
    /// Z njim se izognemo prirejanju poizvedb za vsak posamezen tip podatkovnih struktur.
    /// 
    /// Ne pozabite vključiti paketa System.Linq!
    /// </summary>

    public class Animal
    {
        public int ID { get; set; }
        public string Species { get; set; }
        public bool HasTail { get; set; }
        public int NumberOfLegs { get; set; }

        public override string ToString()
        {
            return $"{this.Species} ({this.NumberOfLegs})"; /*{(this.HasTail ? "ima rep" : "nima repa")}  in " +
                $"{(this.NumberOfLegs == 2 ? "dve nogi" : (this.NumberOfLegs == 4 ? "štiri noge" : this.NumberOfLegs + " nog"))}";*/
        }
    }

    public class Continent
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public List<int> Animals { get; set; }
    }

    public static class LINQDataSet
    {
        // Pripravimo si podatke, po katerih bomo delali poizvedbe
        public static List<Animal> animals = new List<Animal>()
                {
                    new Animal(){ ID=1, Species = "Pes",        HasTail = true,     NumberOfLegs = 4 },
                    new Animal(){ ID=2, Species = "Mačka",      HasTail = true,     NumberOfLegs = 4 },
                    new Animal(){ ID=3, Species = "Papiga",     HasTail = true,     NumberOfLegs = 2 },
                    new Animal(){ ID=4, Species = "Pajek",      HasTail = false,    NumberOfLegs = 8 },
                    new Animal(){ ID=5, Species = "Mravlja",    HasTail = false,    NumberOfLegs = 6 },
                    new Animal(){ ID=6, Species = "Ris",        HasTail = true,     NumberOfLegs = 4 },
                    new Animal(){ ID=7, Species = "Človek",     HasTail = false,    NumberOfLegs = 2 },
                    new Animal(){ ID=8, Species = "Kapitan Kljuka", HasTail = false, NumberOfLegs = 1 },
                    new Animal(){ ID=9, Species = "Orangutan",  HasTail = false,    NumberOfLegs = 2 },
                    new Animal(){ ID=10, Species = "Lev",       HasTail = true,     NumberOfLegs = 4 },
                    new Animal(){ ID=11, Species = "Tiger",     HasTail = true,     NumberOfLegs = 4 },
                };

        public static List<Continent> continents = new List<Continent>()
                {
                    new Continent(){ Name="Afrika", Animals = new List<int>(){ 1, 2, 3, 4, 5, 7, 10 } },
                    new Continent(){ Name="Evropa", Animals = new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 11 } },
                    new Continent(){ Name="Azija", Animals = new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 9, 10 } },
                    new Continent(){ Name="Severna Amerika", Animals = new List<int>(){ 1, 2, 3, 4, 5, 6, 7, 8 } },
                };
    }

    public static class Extensions
    {
        public static void ReadEnumerable<T>(this IEnumerable<T> list)
        {
            Console.Write("Elementi seznama so: ");
            int count = 0;
            foreach (var item in list)
            {
                count++;
                Console.Write(item.ToString() + $"{(count == list.Count() ? "" : ",")} ");
            }
            Console.WriteLine();
        }
    }
}
