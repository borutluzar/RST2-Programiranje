using System;

namespace RST2_Programiranje
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //PrepareOutputs();
            MyStaticFunctions.MakeRandomList(10, out int odds, true);
            Console.WriteLine($"Število lihih vnosov: {odds}");
            Console.Read();
        }

        static void PrepareOutputs()
        {
            int n = 22;
            // Klic funkcije iz drugega razreda
            int primes = MyStaticFunctions.CountPrimes(n);

            // Običajen izpis
            Console.WriteLine();
            Console.WriteLine("Običajen izpis");
            Console.WriteLine("Število praštevil med 1 in "+ n +" je " + primes + ".");
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
    }
}
