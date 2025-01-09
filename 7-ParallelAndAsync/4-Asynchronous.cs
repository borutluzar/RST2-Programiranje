using CommonFunctions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelAndAsync
{
    /// <summary>
    /// Arh, Q63
    /// Tudi: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
    /// Posvetimo se sedaj asinhronemu programiranju.
    /// Priporočeni vzorec asinhronega programiranja je trenutno
    /// task-based asynchronous programming (TAP).
    /// </summary>
    public class Asynchronous
    {
        public enum ExecutionType
        {
            Serial,
            ParallelWrong,
            Parallel
        }

        /// <summary>
        /// Ker v metodi uporabljamo rezervirano besedo 'await', moramo
        /// metodi dodati določilo async.
        /// </summary>
        public static async void AsyncTest()
        {
            // Spet bomo uporabljali razred Task iz knjižnice System.Threading.Tasks
            // Task predstavlja operacijo, ki teče v ozadju (asinhrono ali v drugi niti)

            // Ponovimo primer iz drugega razdelka tega poglavja:
            Stopwatch sw = Stopwatch.StartNew();
            var backgroundTask = Task.Run(() => CountPrimes(InterfaceFunctions.ChooseSection<ExecutionType>()));


            // PRIMER 1: Ko dostopimo do rezultata, bo trenutna nit počakala na rezultat (enako kot pri metodi Wait).            
            /*
            Console.WriteLine($"Task smo zagnali, zdaj čakamo na rezultat.");
            var result = backgroundTask.Result;
            Console.WriteLine($"Čakamo, da task konča izračun.");
            Console.WriteLine($"In čakamo...");
            Thread.Sleep(100);
            Console.WriteLine($"In čakamo...");
            Thread.Sleep(100);
            Console.WriteLine($"Našli smo {result} praštevil v {sw.Elapsed.TotalSeconds} sekundah.");            
            */

            // PRIMER 2: Namesto čakanja lahko uporabimo rezervirano besedo await, 
            //           vendar nam v funkciji to ne spremeni obnašanja.
            //           Se pa bo sprostila trenutna nit za ostale aktivnosti
            
            Console.WriteLine($"Task smo zagnali, zdaj čakamo na rezultat.");            
            var result = await backgroundTask;
            Console.WriteLine($"Čakamo, da task konča izračun.");
            Console.WriteLine($"In čakamo...");
            Console.WriteLine($"In čakamo...");
            Console.WriteLine($"Našli smo {result} praštevil v {sw.Elapsed.TotalSeconds} sekundah.");
            Console.WriteLine($"Do tukaj pridemo šele, ko imamo rezultat");
            

            // PRIMER 3: Implementirajmo našo logiko v ločeni metodi
            //           Klic deluje podobno kot pri await - le da se izvajanje nadaljuje samo znotraj metode
            /*
            Console.WriteLine($"Task smo zagnali, zdaj čakamo na rezultat.");
            var result = ResultAsync();
            Console.WriteLine($"Čakamo, da task konča izračun.");
            Console.WriteLine($"In čakamo...");
            Console.WriteLine($"In čakamo...");
            Console.WriteLine($"Našli smo {result.Result} praštevil v {sw.Elapsed.TotalSeconds} sekundah.");
            Console.WriteLine($"Do tukaj pridemo šele, ko imamo rezultat");
            */

            // Za konec velja pripomniti, da lahko z enim taskom opravimo več zaporednih opravil,
            // kar nam omogoča metoda ContinueWith. Ko se eno opravilo konča, lahko začnemo z naslednjim.
        }

        private static async Task<int> ResultAsync()
        {
            var backgroundTask = Task.Run(() => CountPrimes(InterfaceFunctions.ChooseSection<ExecutionType>()));
            return await backgroundTask;
        }

        private static int CountPrimes(ExecutionType type)
        {
            Int32 count = 0;

            switch (type)
            {
                // Neparalelno
                case ExecutionType.Serial:
                    {
                        DataForParallel.Instance().ForEach(i => { if (CommonFunctions.IsPrime(i)) count++; });
                    }
                    break;
                // Paralelno, vendar narobe
                case ExecutionType.ParallelWrong:
                    {
                        DataForParallel.Instance().AsParallel()
                            .WithDegreeOfParallelism(5)
                            .ForAll(i => 
                                { 
                                    if (CommonFunctions.IsPrime(i)) count++; 
                                });
                           }
                    break;
                // Pravilno paralelno
                case ExecutionType.Parallel:
                    {
                        // Vrednostnih tipov ne moremo zakleniti, zato uporabimo Increment v razredu Interlocked
                        DataForParallel.Instance().AsParallel()
                            .WithDegreeOfParallelism(5)
                            .ForAll(i =>
                                {
                                    if (CommonFunctions.IsPrime(i))
                                        Interlocked.Increment(ref count);
                                });
                        }
                    break;
            }
            return count;
        }


        /// <summary>
        /// Včasih želimo na delo poslati več opravil, 
        /// nato pa čakamo na rezultat zgolj enega ali vseh.
        /// </summary>
        public static async void AsyncTestSeveral(string keyword)
        {
            var backgroundTasks = new[]
            {
                Task.Run(() => OccurencesOnPage(keyword.ToLower(), @"https://scholar.google.si/citations?user=JZ1KGTIAAAAJ&hl=sl&oi=ao")),
                Task.Run(() => OccurencesOnPage(keyword.ToLower(), "https://sl.wikipedia.org/wiki/Deseti_brat")),
                Task.Run(() => OccurencesOnPage(keyword.ToLower(), "https://www.delo.si/novice/slovenija/vecni-vandrovec-v-bitki-proti-dolgcasu/")),
                Task.Run(() => OccurencesOnPage(keyword.ToLower(), "https://www.fis.unm.si/znanstvene-usmeritve/"))
            };

            Console.WriteLine($"Taske smo zagnali, zdaj čakamo na rezultat.");

            // PRIMER 1: Počakamo, da konča vsaj en od taskov
            
            var firstCompleted = await Task.WhenAny(backgroundTasks);
            Console.WriteLine($"\nPrvi je končal task (ID: {firstCompleted.Id}) z rezultatom {firstCompleted.Result}.\n");
            

            // PRIMER 2: Počakamo, da končajo vsi taski
            /*
            var whichIsCompleted = await Task.WhenAll(backgroundTasks);
            Console.WriteLine($"\nVsi taski so se uspešno zaključili, prvi rezultat je bil {whichIsCompleted[0]}.\n");
            */
        }

        private static int OccurencesOnPage(string keyword, string url)
        {
            using var myClient = new HttpClient();
            var response = myClient.GetStreamAsync(url);

            int counter = 0;
            StreamReader sr = new StreamReader(response.Result);
            while (!sr.EndOfStream)
            {
                var words = sr.ReadLine().Split();
                words.ToList().ForEach(w => { if (w.ToLower().Contains(keyword.ToLower())) counter++; });
            }
            response.Dispose();

            Console.WriteLine($"\nPoročam o številu pojavov besede {keyword} na strani {url}: {counter}");
            return counter;
        }
    }
}
