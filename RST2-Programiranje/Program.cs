using System;
using System.Collections.Generic;

namespace Uvod
{
    /// <summary>
    /// Za več in bolj podrobna navodila si oglejte stran
    /// https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/
    /// 
    /// Dodatno o posebnostih zadnjih verzij jezika najdete tudi tukaj:
    /// https://www.dotnetcurry.com/csharp/1489/csharp-8-visual-studio-2019
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //PrepareOutputs();

            //List<int> lstRnd = MyStaticFunctions.MakeRandomList(13, out int odds);
            //Console.WriteLine($"Število lihih vnosov: {odds}");
            
            // Če ne potrebujemo out spremenljivke, uporabimo podčrtaj
            //MyStaticFunctions.MakeRandomList(13, out _);

            //MyStaticFunctions.WriteFile("PrvaPredavanjaTest.txt");

            /*int numbeOfLines = MyStaticFunctions.ReadFile("PrvaPredavanjaTest.txt");
            Console.WriteLine($"Datoteka vsebuje {numbeOfLines} vrstic!");*/

            /*FileData fd = MyStaticFunctions.ReadFile2("PrvaPredavanja.txt");
            Console.WriteLine($"Datoteka vsebuje {fd.NumberOfLines} vrstic in " +
                    $"{(fd.ContainsSensitiveInfo ? "vsebuje moje ime!" : "ne vsebuje mojega imena!")}");*/

            CreateStudents();

            Console.Read();
        }

        /// <summary>
        /// Malo o pripravi nizov za izpis
        /// </summary>
        static void PrepareOutputs()
        {
            int n = 22;
            // Klic funkcije iz drugega razreda
            int primes = MyStaticFunctions.CountPrimes(n);

            // Običajen izpis
            Console.WriteLine();
            Console.WriteLine("Običajen izpis");
            Console.WriteLine("Število praštevil med 1 in " + n + " je " + primes + ".");
            Console.WriteLine("Število praštevil med 1 in {0} je {1}.", n, primes);

            // Interpolacija nizov - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
            Console.WriteLine();
            Console.WriteLine("Izpis z interpolacijo");
            Console.WriteLine($"Število praštevil med 1 in {n} je {primes}.");

            // Pri interpolaciji imamo še dodatne možnosti, npr. zamike
            Console.WriteLine();
            Console.WriteLine("Zamiki pri interpolaciji");
            Console.WriteLine($"|{"Ime in priimek",-20}|{"Ocena",6}|");
            Console.WriteLine($"|{"Borut Lužar",-20}|{"6",6}|");
        }

        static void CreateStudents()
        {
            Student marko = new Student("Marko", "Markantni", new DateTime(1980, 1, 1))
            {
                Subjects = new System.Collections.Generic.List<Subject>() { Subject.DiskretnaMatematika }
            };
            //Console.WriteLine($"{marko.FirstName} je rojen {marko.BirthDate.Day}. {marko.BirthDate.Month}. {marko.BirthDate.Year}");
            Console.WriteLine($"{marko.FirstName} je rojen.");
            Console.WriteLine($"{marko.FirstName} je star {marko.GetAge()} let.");
        }
    }
}
