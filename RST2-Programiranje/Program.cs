using CommonFunctions;
using RST.Math;
using System;
using System.Collections.Generic;

namespace Uvod
{
    /// <summary>
    /// Za več in bolj podrobna navodila o sintaksi C# si oglejte stran
    /// https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/
    /// 
    /// Dodatno o posebnostih zadnjih verzij jezika najdete tudi tukaj:
    /// https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11
    /// https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10
    /// https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            // Poženemo primer glede na izbrano sekcijo
            // Uporabimo splošno generično funkcijo ChooseSection,
            // ki smo jo pripravili v razredni knjižnici (class library) in izpiše možne vrednosti za
            // poljubno enumeracijo.
            switch (InterfaceFunctions.ChooseSection<IntroductorySection>())
            {
                case IntroductorySection.HelloWorld:
                    { // V tem primeru si ogledamo preprosto metodo, ki naredi izpis v konzolo
                        HelloWorld();
                    }
                    break;
                case IntroductorySection.CountingPrimes:
                    { // V tem primeru si ogledamo glavno sintakso C# (zanke, izbirne stavke)
                        int numPrimes = MyStaticFunctions.CountPrimes(20);
                        Console.WriteLine(numPrimes);
                    }
                    break;
                case IntroductorySection.PreparingOutputs:
                    { // V tem primeru si ogledamo možnosti izpisa z uporabo interpolacije
                        PrepareOutputs();
                    }
                    break;
                case IntroductorySection.CountingPrimesWithOut:
                    { // V tem primeru si ogledamo vračanje vrednosti z uporabo določila out
                        int upToNumber = 90;
                        int numPrimes = MyStaticFunctions.CountPrimes(upToNumber, out int largest);
                        Console.WriteLine($"Od 1 do {upToNumber} je {numPrimes} praštevil, največje pa je {largest}.");
                    }
                    break;
                case IntroductorySection.CountingPrimesWithRef:
                    { // V tem primeru si ogledamo vračanje vrednosti z uporabo določila ref
                        int upToNumber = 90;
                        int numPrimes = MyStaticFunctions.CountPrimes(ref upToNumber, out int largest);
                        Console.WriteLine($"Od 1 do {upToNumber} je {numPrimes} praštevil, največje pa je {largest}.");
                        Console.WriteLine($"Vrednost parametra {nameof(upToNumber)} je {upToNumber}!");
                    }
                    break;
                case IntroductorySection.CountingPrimesAndTuples:
                    { // V tem primeru si ogledamo vračanje vrednosti s pomočja strukture Tuple
                      // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
                        int upToNumber = 20;
                        (int numPrimes, int largest) = MyStaticFunctions.CountPrimesAndFindLargest(upToNumber);
                        Console.WriteLine($"Od 1 do {upToNumber} je {numPrimes} praštevil, največje pa je {largest}.");
                    }
                    break;
                case IntroductorySection.Enumerations:
                    { // V tem primeru si ogledamo enumeracije
                      // Analiziramo enumeracijo IntroductorySection
                      // in funkcijo ChooseSection, ki jo kličemo v glavi glavnega switcha
                    }
                    break;
                case IntroductorySection.RandomLists:
                    { // V tem primeru si ogledamo
                      // - generiranje naključnih števil
                      // - uporabo seznamov
                      // - uporabo neobveznih parametrov
                      // - uporabo lambda izrazov (k njim se vrnemo še kasneje)
                        List<int> lstRnd = MyStaticFunctions.MakeRandomList(13, out int odds, true);
                        Console.WriteLine($"\nŠtevilo vnosov je {lstRnd.Count}, od teh je lihih {odds}");
                    }
                    break;
                case IntroductorySection.RandomListsWithoutOut:
                    { // V tem primeru si ogledamo
                      // - kako se izogniti definiranju out spremenljivke, če je ne uporabljamo
                        List<int> lstRnd = MyStaticFunctions.MakeRandomList(13, out _);
                        Console.WriteLine($"\nŠtevilo vnosov je {lstRnd.Count}");
                    }
                    break;
                case IntroductorySection.RandomListsAndYield:
                    { // V tem primeru si ogledamo uporabo yield v zanki. 
                      // Funkcija MakeRandomListWithYield vrača vrednosti takoj, ko jih pridobi
                        foreach (var rnd in MyStaticFunctions.MakeRandomListWithYield(13))
                        {
                            Console.WriteLine($"\nTrenutni vnos je {rnd}");
                        }
                    }
                    break;
                case IntroductorySection.WritingInFile:
                    { // V tem primeru si ogledamo zapisovanje v datoteko
                        MyStaticFunctions.WriteFile("PrvaPredavanjaTest3.txt");
                    }
                    break;
                case IntroductorySection.ReadingFromFile:
                    { // V tem primeru si ogledamo branje iz datoteke
                        int numLines = MyStaticFunctions.ReadFile("PrvaPredavanjaTest2.txt");
                        Console.WriteLine($"\nDatoteka vsebuje {numLines} vrstic");
                    }
                    break;
                case IntroductorySection.ReadingFromFileWithObject:
                    { // V tem primeru si ogledamo branje iz datoteke in
                      // - zapisovanje lastnosti v poseben objekt
                      // - lastnosti in njihove posebnosti (k njim se vrnemo malo kasneje)
                        FileData fileData = MyStaticFunctions.ReadFile2("PrvaPredavanjaTest.txt");
                        Console.WriteLine($"Datoteka vsebuje {fileData.NumberOfLines} vrstic in " +
                                $"{(fileData.ContainsSensitiveInfo ? "vsebuje moje ime!" : "ne vsebuje mojega imena!")}");
                    }
                    break;
                case IntroductorySection.RecallingObjects:
                    { // V tem primeru si osvežimo spomin na definiranje novega tipa (objekta)
                        CreateStudents();
                    }
                    break;
                case IntroductorySection.OtherExamples:
                    { // V tem delu kličemo funkcije iz projekta z živimi primeri
                        int maxDegree = GraphTheory.MaximumDegree(new List<(int end1, int end2)>()
                            {
                                (1, 2),
                                (1, 4),
                                (1, 8),
                                (2, 3),
                                (2, 5),
                                (3, 8),
                                (4, 6),
                                (5, 6)
                            },
                            out int? maxDegreeVert);
                        Console.WriteLine($"Maksimalna stopnja danega grafa je {maxDegree}, doseže pa jo vozlišče {maxDegreeVert}");
                    }
                    break;
            }

            Console.Read();
        }

        /// <summary>
        /// Metoda, ki izpiše Hello World!
        /// </summary>
        public static void HelloWorld()
        {
            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// Malo o pripravi nizov za izpis
        /// </summary>
        static void PrepareOutputs()
        {
            const int n = 22;
            // Klic funkcije iz drugega razreda
            int primes = MyStaticFunctions.CountPrimes(n);

            // Običajen izpis
            Console.WriteLine();
            Console.WriteLine("Običajen izpis z združevanjem:");
            Console.WriteLine("\tŠtevilo praštevil med 1 in " + n + " je " + primes + ".");
            Console.WriteLine("ali s parametrizacijo:");
            Console.WriteLine("\tŠtevilo praštevil med 1 in {0} je {1}.", n, primes);

            // Interpolacija nizov - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
            Console.WriteLine();
            Console.WriteLine("Izpis z interpolacijo");
            // Pri interpolaciji pred nizom zapišemo znak $
            Console.WriteLine($"\tŠtevilo praštevil med 1 in {n} je {primes}.");

            // Kot pri ostalih jezikih pa poznamo tudi 'verbatim' znak @
            Console.WriteLine($@"\tŠtevilo praštevil med 1 in {n} je {primes}.");

            // Z njim lahko definiramo tudi imena spremenljivk iz rezerviranih besed
            int @class = 3;
            Console.WriteLine($"\tVrednost spremenljivke {nameof(@class)} je {@class}.");

            // Pri interpolaciji imamo še dodatne možnosti, npr. zamike
            Console.WriteLine();
            Console.WriteLine("Zamiki pri interpolaciji");
            // Zamik pomeni, da zapis zasede vsaj toliko znakov, kot določimo za vejico
            // Negativna vrednost pomeni levo poravnavo, pozitivna desno
            Console.WriteLine($"\t|{"Ime in priimek",-20}|{"Ocena",6}|");
            Console.WriteLine($"\t|{"Borut Lužar",-20}|{"6  ",6}|");


            Console.WriteLine();
            Console.WriteLine("Določanje formata izpisa pri interpolaciji");
            // Določamo lahko tudi format izpisa, ki ga zapišemo za dvopičjem
            Console.WriteLine($"\t|{"Ime in priimek",-20}|{"Ocena",6}|{"Datum opravljanja",-20}|");
            Console.WriteLine($"\t|{"Borut Lužar",-20}|{"6  ",6}|{DateTime.Now,-20:d. M. yyyy}|");
        }

        /// <summary>
        /// V tej funkciji naredimo novo instanco tipa Student 
        /// in  ji določimo nekaj lastnosti
        /// </summary>
        static void CreateStudents()
        {
            // Kreiramo novo instanco
            Student marko = new Student("Marko", "Markantni", new DateTime(1980, 1, 1))
            {
                Subjects = new System.Collections.Generic.List<Subject>() 
                    { 
                        Subject.DiskretnaMatematika,
                        Subject.Programiranje
                    }   
            };

            //Console.WriteLine($"{marko.FirstName} je rojen {marko.BirthDate.Day}. {marko.BirthDate.Month}. {marko.BirthDate.Year}");
            Console.WriteLine($"{marko.FirstName} je rojen.");
            Console.WriteLine($"{marko.FirstName} je star {marko.GetAge()} let.");
        }

        /// <summary>
        /// Definiramo enumeracijo, s katero si razdelimo razdelke prvega poglavja
        /// </summary>
        private enum IntroductorySection
        {
            HelloWorld = 1,
            CountingPrimes = 2,
            PreparingOutputs = 3,
            CountingPrimesWithOut = 4,
            CountingPrimesWithRef = 5,
            CountingPrimesAndTuples = 6,
            Enumerations = 7,
            RandomLists = 8,
            RandomListsWithoutOut = 9,
            RandomListsAndYield = 10,
            WritingInFile = 11,
            ReadingFromFile = 12,
            ReadingFromFileWithObject = 13,
            RecallingObjects = 14,
            OtherExamples = 99,
        }
    }
}
