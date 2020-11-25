using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
    class Asynchronous
    {
        /// <summary>
        /// Ker v metodi uporabljamo rezervirano besedo 'await', moramo
        /// metodi dodati določilo async.
        /// </summary>
        public static async void AsyncTest()
        {
            // Spet bomo uporabljali razred Task iz knjižnice System.Threading.Tasks
            // Task predstavlja operacijo, ki teče v ozadju (asinhrono ali v drugi niti)

            // Ponovimo primer iz drugega razdelka tega poglavja:
            DateTime dtStart = DateTime.Now;
            var backgroundTask = Task.Run(() => CountPrimes());

            
            Console.WriteLine($"Task smo zagnali, zdaj čakamo na rezultat.");
            // Ko dostopimo do rezultata, bo trenutna nit počakala na rezultat (enako kot pri metodi Wait).
            /*
            var result = backgroundTask.Result;
            DateTime dtEnd = DateTime.Now;
            Console.WriteLine($"Našli smo {result} praštevil v {(dtEnd - dtStart).TotalSeconds} sekundah.");
            */

            // Namesto čakanja lahko uporabimo rezervirano besedo await, 
            // vendar nam v funkciji to ne spremeni obnašanja.
            // Se pa bo sprostila trenutna nit za ostale aktivnosti
            /*
            Console.WriteLine($"Task smo zagnali, zdaj čakamo na rezultat.");            
            var result = await backgroundTask;
            DateTime dtEnd = DateTime.Now;
            Console.WriteLine($"Našli smo {result} praštevil v {(dtEnd-dtStart).TotalSeconds} sekundah.");
            */

            // Implementirajmo našo logiko v ločeni metodi
            Console.WriteLine($"Task smo zagnali, zdaj čakamo na rezultat.");
            DateTime dtStart3 = DateTime.Now;
            var result = ResultAsync();
            Console.WriteLine($"Čakamo, da task konča izračun.");
            Console.WriteLine($"In čakamo...");            
            Console.WriteLine($"In čakamo...");
            Console.WriteLine($"Našli smo {result.Result} praštevil.");
            DateTime dtEnd3 = DateTime.Now;
            Console.WriteLine($"V {(dtEnd3-dtStart3).TotalSeconds} sekundah.");


            // Za konec velja pripomniti, da lahko z enim taskom opravimo več zaporednih opravil,
            // kar nam omogoča metoda ContinueWith. Ko se eno opravilo konča, lahko začnemo z naslednjim.
        }

        private static async Task<int> ResultAsync()
        {
            var backgroundTask = Task.Run(() => CountPrimes());
            return await backgroundTask;
        }

        private static int CountPrimes()
        {
            Int32 count = 0;

            // Neparalelno
            //DataForParallel.Instance().ForEach(i => { if (CommonFunctions.IsPrime(i)) count++; });

            // Paralelno, vendar narobe
            /*DataForParallel.Instance().AsParallel()
                .WithDegreeOfParallelism(3)
                .ForAll(i => { if (CommonFunctions.IsPrime(i)) count++; });*/

            // Pravilno paralelno
            // Vrednostnih tipov ne moremo zakleniti, zato uporabimo Increment v razredu Interlocked
            DataForParallel.Instance()
                .AsParallel()
                .ForAll(i => { if (CommonFunctions.IsPrime(i)) 
                        Interlocked.Increment(ref count); });
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
                Task.Run(() => OccurencesOnPage(keyword.ToLower(), "https://www.researchgate.net/profile/Borut_Luar")),
                Task.Run(() => OccurencesOnPage(keyword.ToLower(), "https://sl.wikipedia.org/wiki/Deseti_brat")),
                Task.Run(() => OccurencesOnPage(keyword.ToLower(), "https://www.delo.si/novice/slovenija/vecni-vandrovec-v-bitki-proti-dolgcasu/")),
                Task.Run(() => OccurencesOnPage(keyword.ToLower(), "https://www.fis.unm.si/si/o-fakulteti/nasa-zgodba/"))                
            };

            Console.WriteLine($"Taske smo zagnali, zdaj čakamo na rezultat.");
            var firstCompleted = await Task.WhenAny(backgroundTasks);
            //var whichIsCompleted = await Task.WhenAll(backgroundTasks);
            //Console.WriteLine($"\nPrvi je končal task (ID: {firstCompleted.Id}) z rezultatom {firstCompleted.Result}.\n");
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
