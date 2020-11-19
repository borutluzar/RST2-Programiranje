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
                        if (CommonFunctions.IsPrime(i))
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
                if (CommonFunctions.IsPrime(i))
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
                if (CommonFunctions.IsPrime(i))
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
            dtEnd = DateTime.Now;
            ts = dtEnd - dtStart;
            Console.WriteLine($"Čas vzporednega iskanja s tremi jedri: {ts.TotalSeconds}, našli smo {primes.Count} praštevil.");
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
            Console.WriteLine($"Prvih 100 praštevil:");
            primes.Take(100).ReadEnumerable();

            primes.Clear();
            DataForParallel.Instance().AsParallel().ForAll(i =>
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
            if (CommonFunctions.IsPrime(i))
                return (i, true);
            return (i, false);
        }
    }
}
