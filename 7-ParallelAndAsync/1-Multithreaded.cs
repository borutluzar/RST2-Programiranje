using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelAndAsync
{
    /// <summary>
    /// Arh, Q59, Q61
    /// 
    /// V tem poglavju si bomo podrobneje ogledali paralelno programiranje.
    /// Preden začnemo, razložimo tri izraze, ki se pojavljajo s tem v zvezi.
    /// 
    /// Hkratno programiranje (concurrent programming):
    ///     pomeni, da se več operacij lahko izvaja hkrati. 
    ///     Naslednja dva tipa sta podkategoriji hkratnega programiranja.
    ///     
    /// Večnitno programiranje (multithreaded programming):
    ///     je programiranje, ko se program hkrati izvaja v več ločenih nitih.
    ///     Izvajanje vsake od niti določa in nadzoruje operacijski sistem.
    ///     Če želimo poganjati več niti, kot imamo jeder na procesorju,
    ///     potem operacijski sistem stalno preklaplja med nitmi.
    ///     
    /// Asinhrono programiranje (asynchronous programming):
    ///     uporabljamo ga za I/O-odvisno delo. Pri njem ne gre za zahtevne računske procese,
    ///     na katere moramo čakati s polno zasedenim procesorjem,
    ///     temveč za procese, ki se npr. povezujejo na bazo podatkov, 
    ///     tam izvajajo poizvedbe ter vračajo rezultate, kar lahko traja nekaj časa.
    ///     Če za naše delo v danem trenutku podatki, ki se pridobivajo, niso ključni,
    ///     lahko naš program izvaja svoje delo naprej.
    /// </summary>
    class Multithreads
    {
        private static long x = 0;
        private const int WAIT_INTERVAL = 6000; // V milisekundah

        /// <summary>
        /// Za enostavno večnitno programiranje lahko uporabimo razred Thread (iz paketa System.Threading).
        /// </summary>
        public static void Multithreaded()
        {
            // Pripravimo novo nit in ji povejmo, kaj naj izvaja
            Thread thread1 = new Thread(ComputeLong);
            Thread thread2 = new Thread(ComputeLong);
            // Kot delegata lahko podamo tudi funkcije s parametri, vendar morajo biti tipa object
            Thread thread3 = new Thread(ComputeLongWithIncrement);

            // Oglejmo si delovanje procesorja v Task Managerju.
            Console.WriteLine($"\nPočakamo {WAIT_INTERVAL / 1000} sekund.");
            Thread.Sleep(WAIT_INTERVAL); // Zamrznemo izvajanje programa v naši trenutni niti.            
            Console.WriteLine("\nZačenjamo s prvo nitjo:");
            // Zaženemo jo s funkcijo Start.
            thread1.Start();

            Console.WriteLine($"\nPočakamo še {WAIT_INTERVAL / 1000} sekund.");
            Thread.Sleep(WAIT_INTERVAL); // Zamrznemo izvajanje programa v naši trenutni niti.
            Console.WriteLine("\nZačenjamo z drugo nitjo:");
            // Zaženemo jo s funkcijo Start.
            thread2.Start();

            Console.WriteLine($"\nPočakamo še {WAIT_INTERVAL / 1000} sekund.");
            Thread.Sleep(WAIT_INTERVAL); // Zamrznemo izvajanje programa v naši trenutni niti.
            Console.WriteLine("\nZačenjamo s tretjo nitjo:");
            // Funkciji Start podamo še pričakovani parameter za funkcijo ComputeLongWithIncrement
            thread3.Start(WAIT_INTERVAL / 2);

            Console.WriteLine("\nV trenutni niti še vedno lahko izvajamo svoje operacije.");


            // Z metodo Join pa ustavimo (blokiramo) trenutno nit, dokler thread1 ne konča z izvajanjem.
            //thread1.Join();
            //Console.WriteLine("\nProgram se zaključi zdaj");            

            // Druga možnost je, da metodi Join povemo,
            // koliko časa smo pripravljeni čakati, ona pa pove, če se je v tem času proces zaključil     
            bool isDone = thread1.Join(WAIT_INTERVAL);
            if (isDone)
                Console.WriteLine("\nProces 1 se je zaključil!");
            else
            {
                Console.WriteLine("\nNaveličali smo se čakati.");
                // Včasih se je uporabljala funkcija Abort, 
                // vendar je v .Net Core ogrodju onemogočena, 
                // zaradi različnih težav, ki jih lahko njena uporaba povzroči.
                //thread1.Abort();


                // Uporaba interrupt ne ubije niti, ki se izvajajo v ozadju.
                thread1.Interrupt();
                thread2.Interrupt();
                thread3.Interrupt();
                Console.WriteLine("\nVsi procesi so ustavljeni. (Ne bo držalo...)");
                return;
            }
        }

        private static void ComputeLong()
        {
            long limit = long.MaxValue;
            while (x < limit)
                x++;
        }

        private static void ComputeLong(CancellationToken token)
        {
            long limit = long.MaxValue;
            while (x < limit)
            {
                if (!token.IsCancellationRequested)
                    x++;
                else
                    break;
            }
            Console.WriteLine($"Prišli smo do konca zanke z x = {x}");
        }

        private static void ComputeLongWithIncrement(object increment)
        {
            long limit = long.MaxValue;
            while (x < limit)
                x += (int)increment;
        }

        /// <summary>
        /// Uporaba knjižnice Threading se ne priporoča več.
        /// Obstaja nekaj drugih knjižnic za upravljanje z nitmi.
        /// Najbolj se (zaenkrat) priporoča knjižnica Task Parallel Library (spada pod System.Threading.Tasks).
        /// </summary>
        public static void Tasks()
        {
            // Ponovimo, kar smo želeli doseči s knjižnico Threading.

            // Oglejmo si delovanje procesorja v Task Managerju.
            Console.WriteLine($"\nPočakamo {WAIT_INTERVAL / 1000} sekund.");

            // Definiramo še "flag", ki nam omogoči predčasno končanje procesa
            CancellationTokenSource tokenSource = new CancellationTokenSource(); // Arh, Q63

            Thread.Sleep(WAIT_INTERVAL); // Zamrznemo izvajanje programa v naši trenutni niti.            
            Console.WriteLine("\nZačenjamo s prvim opravilom:");
            var task1 = Task.Run(() => ComputeLong(tokenSource.Token));

            Thread.Sleep(WAIT_INTERVAL); // Zamrznemo izvajanje programa v naši trenutni niti.            
            Console.WriteLine("\nZačenjamo z drugim opravilom:");
            var task2 = Task.Run(() => ComputeLong(tokenSource.Token));

            Thread.Sleep(WAIT_INTERVAL); // Zamrznemo izvajanje programa v naši trenutni niti.            
            Console.WriteLine("\nZačenjamo s tretjim opravilom:");
            var task3 = Task.Run(ComputeLong);

            // Počakajmo z glavno nitjo, da pomožne niti zaključijo proces
            Thread.Sleep(WAIT_INTERVAL);
            Console.WriteLine("\nPočakajmo na izračun");
            // Funkcijo Join iz prejšnjega primera nadomesti funkcija Wait.                                    
            try
            {
                Console.WriteLine($"Status taska 1 (pred prekinitvijo): {task1.Status}");

                // Prekinimo prvi task
                tokenSource.Cancel();
                task1.Wait();
                //task2.Wait();
                //task3.Wait();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Prišlo je do izjeme OperationCanceledException");
            }
            finally
            {                
                Console.WriteLine($"Status taska 1 (po prekinitvi): {task1.Status}");                
                task1.Dispose();                
                Console.WriteLine($"\n{nameof(task1)} smo uspešno zaključili!");                
            }

            Console.WriteLine("\nProgram se zaključi zdaj");

            // Task neposredno ne predstavlja niti, ampak je razvrščanje 
            // in upravljanje z nitmi prepuščeno upravljalcu opravil (task scheduler).
            // To omogoči boljšo optimizacijo upravljanja kot v prejšnjem primeru.
        }

        /// <summary>
        /// Arh, Q62
        /// Še malo več o uporabi razreda Task
        /// </summary>
        public static void TasksResult()
        {
            Console.WriteLine("\nZaženimo task vzporedno.");
            //var task = Task.Run(ComputeLong);
            //Console.WriteLine($"\nTip {nameof(task)} je {task.GetType().Name}");

            var task = Task.Run(ComputeLongAndReturn); // Poglejmo si razliko v tipu, če funkcija ne vrača ničesar
            Console.WriteLine($"\nTip {nameof(task)} je {task.GetType().Name}");

            // Drug način klica funkcije je lambda izraz, kjer funkciji lahko damo tudi parameter
            //var task = Task.Run(() => ComputeLongWithIncrementAndReturn(2));

            Console.WriteLine("\nMoj program teče vzporedno.");

            Thread.Sleep(1000);
            Console.WriteLine("\nMoj program še vedno teče vzporedno.");

            
            
            // Če funkcija, ki jo task izvaja, vrača rezultat, ga dobimo z lastnostjo Result.
            var result = task.Result;
            // Ko se rezultat pojavi, se naša koda izvaja naprej
            Console.WriteLine($"\nRezultat taska je {result}.");
            

            // Ponovimo: tako metoda Wait kot lastnost Result blokirata izvajanje trenutne niti, dokler se task ne izvede.
        }

        private static long ComputeLongAndReturn()
        {
            long limit = 4_000_000_000;
            while (x < limit)
                x++;

            return x + 1;
        }

        private static long ComputeLongWithIncrementAndReturn(int inc)
        {
            long limit = 6_000_000_000;
            while (x < limit)
                x += inc;

            return x + 1;
        }
    }
}
