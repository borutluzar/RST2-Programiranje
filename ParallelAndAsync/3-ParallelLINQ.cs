using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            // Praštevila iz množice si shranimo v drug seznam
            List<int> primes = new List<int>();

            // Najprej preizkusimo običajni pristop
            DateTime dtStart = DateTime.Now;
            DataForParallel.Instance().ForEach(i =>
                    {
                        if (IsPrime(i))
                            primes.Add(i);
                    });
            DateTime dtEnd = DateTime.Now;
            TimeSpan ts = dtEnd - dtStart;

            Console.WriteLine($"Čas zaporednega iskanja: {ts.TotalSeconds}, našli smo {primes.Count} praštevil.");


            // Sedaj pa jih preverimo paralelno
            primes.Clear();
            dtStart = DateTime.Now;
            // Naš seznam prevedemo v instanco ParallelQuery<T> s klicem funkcije AsParallel.
            // Nato uporabimo funkcijo ForAll, ki naredi enako kot ForEach zgoraj, 
            // le da uporabi več paralelnih niti.
            DataForParallel.Instance().AsParallel().ForAll(i =>
            {
                if (IsPrime(i))
                {
                    // Ker vzoredno dodajamo elemente v seznam (niti si ga delijo),
                    // ga moramo ob vsakem dodajanju 'zakleniti', 
                    // da ga ne uporabi hkrati druga nit in ne pride do manjkajočih vnosov
                    lock (primes)
                    {
                        primes.Add(i);
                    }
                }
            });
            dtEnd = DateTime.Now;
            ts = dtEnd - dtStart;
            Console.WriteLine($"Čas vzporednega iskanja: {ts.TotalSeconds}, našli smo {primes.Count} praštevil.");


            // Če ne uporabimo 'lock', nam nekaj vnosov zmanjka
            primes.Clear();
            dtStart = DateTime.Now;
            DataForParallel.Instance().AsParallel().ForAll(i =>
            {
                if (IsPrime(i))
                {
                    primes.Add(i);
                }
            });
            dtEnd = DateTime.Now;
            ts = dtEnd - dtStart;
            Console.WriteLine($"Čas vzporednega iskanja brez lock-a: {ts.TotalSeconds}, našli smo {primes.Count} praštevil.");


            // Poleg metode ForAll imamo na voljo še nekaj drugih metod. Npr. Select, OrderBy itd.


            // V zgornjih primerih smo uporabili vsa jedra procesorja, kar ni vedno dobrodošlo.
            // Število uporabljenih jeder lahko tudi omejimo z metodo WithDegreeOfParallelism.
            // Preverimo paralelno samo s tremi jedri.
            primes.Clear();
            dtStart = DateTime.Now;
            DataForParallel.Instance().AsParallel()
                    .WithDegreeOfParallelism(3)
                    .ForAll(i =>
                        {
                            if (IsPrime(i))
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
            dtEnd = DateTime.Now;
            ts = dtEnd - dtStart;
            Console.WriteLine($"Čas vzporednega iskanja s tremi jedri: {ts.TotalSeconds}, našli smo {primes.Count} praštevil.");
        }

        public static bool IsPrime(int i)
        {
            for (int j = 2; j <= (int)Math.Sqrt(i); j++)
            {
                if (i % j == 0)
                    return false;
            }
            return true;
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
            // Ta klic na metodo ForAll nima vpliva, ker ne vrača rezultata.
            DataForParallel.Instance().AsParallel().AsOrdered().ForAll(i =>
            {
                if (IsPrime(i))
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
            Console.WriteLine($"Prvih 100 praštevil:");
            primes.Take(100).ReadEnumerable();

            primes.Clear();
            DataForParallel.Instance().AsParallel().ForAll(i =>
            {
                if (IsPrime(i))
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
            Console.WriteLine($"Prvih 100 praštevil brez izbiranja po vrsti:");
            primes.Take(100).ReadEnumerable();


            Console.WriteLine($"\nZ metodo Select:");

            // Uporaba AsOrdered ima vpliv na metodo Select.
            //var resultPrimes = DataForParallel.Instance().AsParallel().AsOrdered().Select<int, int?>(i => IsPrime(i) ? (int?)i : null);
            var resultPrimes = DataForParallel.Instance().AsParallel().AsOrdered().Select(i => ReturnIfPrime(i));
            Console.WriteLine($"Prvih 100 praštevil:");
            resultPrimes.Take(100).ReadEnumerable();

            resultPrimes = DataForParallel.Instance().AsParallel().Select(i => ReturnIfPrime(i));
            Console.WriteLine($"Prvih 100 praštevil:");
            resultPrimes.Take(100).ReadEnumerable();
        }

        private static (int i, bool @is) ReturnIfPrime(int i)
        {
            if (IsPrime(i))
                return (i, true);
            return (i, false);
        }
    }

    class DataForParallel
    {
        private List<int> primeCandidates;
        private const int BOUND = 5000000;
        //private const int BOUND = 10000000;
        //private const int BOUND = 15000000;

        private DataForParallel()
        {
            this.primeCandidates = new List<int>() { 2 };

            for (int i = 3; i < BOUND; i += 2)
            {
                primeCandidates.Add(i);
            }
        }

        private static List<int> instance = null;
        public static List<int> Instance()
        {
            if (instance == null)
                instance = new DataForParallel().primeCandidates;

            return instance;
        }
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
