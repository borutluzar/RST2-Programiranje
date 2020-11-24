using System;
using System.Collections.Generic;
using System.Text;
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
    ///     Izvajanje vsake izmed niti določa in nadzoruje operacijski sistem.
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

        /// <summary>
        /// Za enostavno večnitno programiranje lahko uporabimo razred Thread (iz paketa System.Threading).
        /// </summary>
        public static void Multithreaded()
        {
            // Pripravimo novo nit in ji povejmo, kaj naj izvaja
            Thread thread1 = new Thread(ComputeLong);
            Thread thread2 = new Thread(ComputeLong);
            // Kot delegata lahko podamo tudi funkcijo s parametri, vendar morajo biti tipa object
            Thread thread3 = new Thread(ComputeLongWithIncrement);

            // Oglejmo si delovanje procesorja v Task Managerju.
            Console.WriteLine("\nPočakamo 2 sekundi.");
            Thread.Sleep(2000); // Zamrznemo izvajanje programa v naši trenutni niti.            
            Console.WriteLine("\nZačenjamo s prvo nitjo:");
            // Zaženemo jo s funkcijo Start.
            thread1.Start();

            Console.WriteLine("\nPočakamo še 2 sekundi.");
            Thread.Sleep(2000); // Zamrznemo izvajanje programa v naši trenutni niti.
            Console.WriteLine("\nZačenjamo z drugo nitjo:");
            // Zaženemo jo s funkcijo Start.
            thread2.Start();

            Console.WriteLine("\nPočakamo še 2 sekundi.");
            Thread.Sleep(2000); // Zamrznemo izvajanje programa v naši trenutni niti.
            Console.WriteLine("\nZačenjamo s tretjo nitjo:");
            // Funkciji Start podamo še pričakovani parameter za funkcijo ComputeLongWithIncrement
            thread3.Start(1000);

            Console.WriteLine("\nV trenutni niti še vedno lahko izvajamo svoje operacije.");

            /*
            // Z metodo Join pa ustavimo (blokiramo) trenutno nit, dokler thread1 ne konča z izvajanjem.
            thread1.Join();
            Console.WriteLine("\nProgram se zaključi zdaj");
            */

            // Druga možnost je, da metodi Join povemo,
            // koliko časa smo pripravljeni čakati, ona pa pove, če se je v tem času proces zaključil
            bool isDone = thread1.Join(10000);
            if (isDone)
                Console.WriteLine("\nProces 1 se je zaključil!");
            else
            {
                Console.WriteLine("\nNaveličali smo se čakati.");
                // Bom še preveril in izveste v torek!
                thread1.Suspend();
                thread2.Suspend();
                thread3.Suspend();
            }
        }

        private static void ComputeLong()
        {
            long limit = long.MaxValue;
            while (x < limit)
                x++;
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
        /// Najbolj se priporoča knjižnica Task Parallel Library (spada pod System.Threading.Tasks).
        /// </summary>
        public static void Tasks()
        {
            // Ponovimo, kar smo želeli doseči s knjižnico Threading.

            // Oglejmo si delovanje procesorja v Task Managerju.
            Console.WriteLine("\nPočakamo 2 sekundi.");

            Thread.Sleep(2000); // Zamrznemo izvajanje programa v naši trenutni niti.            
            Console.WriteLine("\nZačenjamo s prvim opravilom:");
            var task1 = Task.Run(ComputeLong);

            Thread.Sleep(2000); // Zamrznemo izvajanje programa v naši trenutni niti.            
            Console.WriteLine("\nZačenjamo z drugim opravilom:");
            var task2 = Task.Run(ComputeLong);

            Thread.Sleep(2000); // Zamrznemo izvajanje programa v naši trenutni niti.            
            Console.WriteLine("\nZačenjamo s tretjim opravilom:");
            var task3 = Task.Run(ComputeLong);

            // Počakajmo z glavno nitjo, da pomožne niti zaključijo proces
            Console.WriteLine("\nPočakajmo na izračun");
            // Funkcijo Join iz prejšnjega primera nadomesti funkcija Wait.
            task1.Wait();
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
            //var task = Task.Run(ComputeLongAndReturn); // Poglejmo si razliko v tipu, če funkcija ne vrača ničesar

            // Drug način klica funkcije je lambda izraz, kjer funkciji lahko damo tudi parameter
            var task = Task.Run(() => ComputeLongWithIncrementAndReturn(2));

            Console.WriteLine("\nMoj program teče vzporedno.");

            // Če funkcija, ki jo task izvaja vrača rezultat, ga dobimo z lastnostjo Result.
            var result = task.Result;
            // Ko se rezultat pojavi, se naša koda izvaja naprej
            Console.WriteLine($"\nRezultat taska je {result}.");

            // Ponovimo: tako metoda Wait kot lastnost Result blokirata izvajanje trenutne niti, dokler se task ne izvede.
        }

        private static long ComputeLongAndReturn()
        {
            long limit = 4000000000;
            while (x < limit)
                x++;

            return x + 1;
        }

        private static long ComputeLongWithIncrementAndReturn(int inc)
        {
            long limit = 6000000000;
            while (x < limit)
                x += inc;

            return x + 1;
        }
    }
}
