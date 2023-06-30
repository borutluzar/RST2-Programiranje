﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ParallelAndAsync
{
    /// <summary>
    /// Arh, Q62
    /// Paralelizacija je učinkovito implementirana tudi za obravnavo zbirk.
    /// </summary>
    class ParallelLINQ
    {
        public static void PLINQExample()
        {
            Console.WriteLine($"Poiščimo praštevila v veliki množici:");

            Console.WriteLine($"Najprej zaporedno:");
            // Praštevila iz množice si shranimo v drug seznam
            List<int> primes = new List<int>();

            // Najprej preizkusimo običajni pristop
            Stopwatch sw = Stopwatch.StartNew();
            int testAdd = 0;
            DataForParallel.Instance().ForEach(i =>
                    {
                        if (CommonFunctions.IsPrime(i))
                            primes.Add(i);
                    });
            Console.WriteLine($"Čas zaporednega iskanja: {sw.Elapsed.TotalSeconds}, našli smo {primes.Count} praštevil.");

            primes.Clear();
            sw = Stopwatch.StartNew();
            int testWhere = DataForParallel.Instance().Where(CommonFunctions.IsPrime).Count();
            Console.WriteLine($"Čas zaporednega iskanja z Where: {sw.Elapsed.TotalSeconds}, " +
                $"našli smo {testWhere} praštevil.");

            Console.WriteLine($"Gremo!");
            sw = Stopwatch.StartNew();
            var testWhereParallel = DataForParallel.Instance().AsParallel().Where(CommonFunctions.IsPrime);
            Console.WriteLine($"Čas vzporednega iskanja z Where: {sw.Elapsed.TotalSeconds}, " +
                $"našli smo {testWhereParallel.Count()} praštevil."); // Hitro, ker se čas izpiše, preden se evaluira Count.

            sw = Stopwatch.StartNew();
            int testWhereParallelCount = DataForParallel.Instance().AsParallel().Where(CommonFunctions.IsPrime).Count();
            Console.WriteLine($"Čas vzporednega iskanja z Where: {sw.Elapsed.TotalSeconds}, " +
                $"našli smo {testWhereParallel} praštevil.");

            Thread.Sleep(1000);

            // Sedaj pa jih preverimo paralelno
            Console.WriteLine($"\nIn še vzporedno:");
            primes.Clear();
            sw = Stopwatch.StartNew();
            // Naš seznam prevedemo v instanco ParallelQuery<T> s klicem funkcije AsParallel.
            // Nato uporabimo funkcijo ForAll, ki naredi enako kot ForEach zgoraj, 
            // le da uporabi več paralelnih niti.
            DataForParallel.Instance().AsParallel().ForAll(i =>
            {
                if (CommonFunctions.IsPrime(i))
                {
                    // Ker vzporedno dodajamo elemente v seznam (niti si ga delijo),
                    // ga moramo ob vsakem dodajanju 'zakleniti', 
                    // da ga ne uporabi hkrati druga nit in ne pride do manjkajočih vnosov
                    lock (primes)
                    {
                        primes.Add(i);
                    }
                }
            });
            Console.WriteLine($"Čas vzporednega iskanja: {sw.Elapsed.TotalSeconds}, našli smo {primes.Count} praštevil.");


            // Če ne uporabimo 'lock', nam nekaj vnosov zmanjka
            primes.Clear();
            sw = Stopwatch.StartNew();
            DataForParallel.Instance().AsParallel().ForAll(i =>
            {
                if (CommonFunctions.IsPrime(i))
                {
                    primes.Add(i);
                }
            });
            Console.WriteLine($"Čas vzporednega iskanja brez lock-a: {sw.Elapsed.TotalSeconds}, našli smo {primes.Count} praštevil.");


            // Poleg metode ForAll imamo na voljo še nekaj drugih metod. Npr. Select, OrderBy itd.


            // V zgornjih primerih smo uporabili vsa jedra procesorja, kar ni vedno dobrodošlo.
            // Število uporabljenih jeder lahko tudi omejimo z metodo WithDegreeOfParallelism.
            // Preverimo paralelno samo s tremi jedri.            
            for (int i = 1; i < Environment.ProcessorCount; i++)
            {
                primes.Clear();
                sw = Stopwatch.StartNew();
                DataForParallel.Instance().AsParallel()
                        .WithDegreeOfParallelism(i)
                        .ForAll(i =>
                            {
                                if (CommonFunctions.IsPrime(i))
                                {
                                // Ker vzoredno dodajamo elemente v seznam,
                                // ga moramo ob vsakem dodajanju 'zakleniti', 
                                // da ga ne uporabi hkrati druga nit in ne pride do manjkajočih vnosov
                                lock (primes)
                                    {
                                        primes.Add(i);
                                    }
                                }
                            });
                Console.WriteLine($"Čas vzporednega iskanja z {i} jedri od {Environment.ProcessorCount}: {sw.Elapsed.TotalSeconds}, našli smo {primes.Count} praštevil.");
            }
        }

        /// <summary>
        /// V tem primeru si bomo ogledali še, kako se problema lotimo, 
        /// če je vrstni red rezultata pomemben.
        /// </summary>
        public static void PLINQExampleOrdered()
        {
            Console.WriteLine($"Poiščimo praštevila v veliki množici:");

            // Praštevila iz množice si shranimo v drug seznam
            List<int> primes = new List<int>();

            // Paralelizacija razdeli našo množico na več delov.
            // To lahko pomeni, da elementi niso obravnavani zaporedoma, 
            // temveč po sklopih, ki so izbrani za vsako nit.
            // Urejenost končnega rezultata zahtevamo s klicem funkcije AsOrdered.
            // !Ta klic na metodo ForAll nima vpliva, ker ne vrača rezultata!
            DataForParallel.Instance().AsParallel().AsOrdered().ForAll(i =>
            {
                if (CommonFunctions.IsPrime(i))
                {
                    // Ker vzporedno dodajamo elemente v seznam,
                    // ga moramo ob vsakem dodajanju 'zakleniti', 
                    // da ga ne uporabi hkrati druga nit in ne pride do manjkajočih vnosov
                    lock (primes)
                    {
                        primes.Add(i);
                    }
                }
            });
            Console.WriteLine($"Prvih 100 praštevil z uporabo AsOrdered (nima vpliva na rezultat pri ForAll):");
            primes.Take(100).ReadEnumerable();

            primes.Clear();
            DataForParallel.Instance().AsParallel().ForAll(i =>
            {
                if (CommonFunctions.IsPrime(i))
                {
                    // Ker vzporedno dodajamo elemente v seznam,
                    // ga moramo ob vsakem dodajanju 'zakleniti', 
                    // da ga ne uporabi hkrati druga nit in ne pride do manjkajočih vnosov
                    lock (primes)
                    {
                        primes.Add(i);
                    }
                }
            });
            Console.WriteLine($"Prvih 100 praštevil brez izbiranja po vrsti:");
            primes.Take(100).ReadEnumerable();


            Console.WriteLine($"\n\nZ metodo Select:");

            // Uporaba AsOrdered ima vpliv na metodo Select.
            //var resultPrimes = DataForParallel.Instance().AsParallel().AsOrdered().Select<int, int?>(i => IsPrime(i) ? (int?)i : null);
            var resultPrimes = DataForParallel.Instance().AsParallel().AsOrdered().Select(i => ReturnIfPrime(i));
            Console.WriteLine($"Prvih 100 praštevil (urejenih):");
            resultPrimes.Take(100).ReadEnumerable();

            resultPrimes = DataForParallel.Instance().AsParallel().Select(i => ReturnIfPrime(i));
            Console.WriteLine($"Prvih 100 praštevil (neurejenih):");
            resultPrimes.Take(100).ReadEnumerable();
        }

        private static (int i, bool @is) ReturnIfPrime(int i)
        {
            if (CommonFunctions.IsPrime(i))
                return (i, true);
            return (i, false);
        }
    }
}
