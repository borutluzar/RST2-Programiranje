using CommonFunctions;
using System;
using System.Collections.Generic;

namespace Uvod
{
    /// <summary>
    /// Za več in bolj podrobna navodila o sintaksi C# si oglejte stran
    /// https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/
    /// 
    /// Dodatno o posebnostih zadnjih verzij jezika najdete tudi tukaj:
    /// https://www.dotnetcurry.com/csharp/1489/csharp-8-visual-studio-2019
    /// </summary>
    class Program
    {
        private enum IntroductorySection
        {
            HelloWorld = 1,
            CountingPrimes = 2,
            PreparingOutputs = 3,
            CountingPrimesWithOut = 4,
            CountingPrimesAndTuples = 5,
            RandomLists = 6,
            RandomListsWithoutOut = 7,
            WritingInFile = 8,
            ReadingFromFile = 9,
            ReadingFromFileWithObject = 10,
            RecallingObjects = 11
        }

        static void Main(string[] args)
        {
            // Poženemo primer glede na izbrano sekcijo
            // Namesto funkcije ChooseExampleSection uporabimo bolj splošno generično funkcijo ChooseSection,
            // ki smo jo pripravili v razredni knjižnici (class library) in izpiše možne vrednosti za
            // poljubno enumeracijo.
            switch (/*ChooseExampleSection()*/ InterfaceFunctions.ChooseSection<IntroductorySection>())
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
                case IntroductorySection.CountingPrimesAndTuples:
                    { // V tem primeru si ogledamo vračanje vrednosti s pomočja strukture Tuple
                      // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
                        int upToNumber = 20;
                        (int numPrimes, int largest) = MyStaticFunctions.CountPrimesAndFindLargest(upToNumber);
                        Console.WriteLine($"Od 1 do {upToNumber} je {numPrimes} praštevil, največje pa je {largest}.");
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
                Subjects = new System.Collections.Generic.List<Subject>() { Subject.DiskretnaMatematika }
            };

            //Console.WriteLine($"{marko.FirstName} je rojen {marko.BirthDate.Day}. {marko.BirthDate.Month}. {marko.BirthDate.Year}");
            Console.WriteLine($"{marko.FirstName} je rojen.");
            Console.WriteLine($"{marko.FirstName} je star {marko.GetAge()} let.");
        }

        /// <summary>
        /// Funkcija izpiše možne sekcije primerov in prebere izbiro uporabnika
        /// </summary>
        private static IntroductorySection ChooseExampleSection()
        {
            // Izpis sekcij za izbiro 
            int i = 1;
            Console.WriteLine("--\t--\t--\t--");
            Console.WriteLine("Example sections:\n");
            foreach (var section in Enum.GetValues(typeof(IntroductorySection)))
            {
                Console.WriteLine($"{i}. {section}");
                i++;
            }
            Console.WriteLine("\n--\t--\t--\t--");
            Console.Write("Choose example section to run: ");

            string input = Console.ReadLine();
            bool isFormatCorrect = int.TryParse(input, out int chosen);
            if (!isFormatCorrect)
            {
                Console.WriteLine($"\n The input {input} is not an integer! The execution is stopped.");
                return 0;
            }
            else if (chosen <= 0 || chosen > i)
            {
                Console.WriteLine($"\n There is no Section {input}! The execution is stopped.");
                return 0;
            }

            Console.Write("\n");
            Console.WriteLine($"Running example section {chosen}...");
            Console.Write("\n\n");
            return (IntroductorySection)chosen;
        }
    }
}
