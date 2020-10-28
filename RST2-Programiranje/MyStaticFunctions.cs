using System;
using System.Collections.Generic;
using System.Text;

namespace RST2_Programiranje
{
    public class MyStaticFunctions
    {
        private const string DEBUG = "Deb>";

        /// <summary>
        /// Counts all prime numbers up to n
        /// </summary>
        /// <param name="n">An integer up to which we count primes</param>
        public static int CountPrimes(int n)
        {
            int countPrimes = 0;

            // For each i between 1 and n check if it is a prime number
            for(int i=2; i<=n; i++)
            {
                // Count number of divisors of the current number i
                int countDivisors = 0;
                for(int j=2; j <= Math.Sqrt(i); j++)
                {
                    if (i % j == 0)
                        countDivisors++;

                    if (countDivisors > 0)
                        break;
                }

                if (countDivisors == 0)
                    countPrimes++;
            }

            return countPrimes;
        }

        /// <summary>
        /// Creates a list of integers with random entries
        /// </summary>
        /// <param name="capacity">Number of elements</param>
        /// <returns>List with random entries</returns>
        public static List<int> MakeRandomList(int capacity, out int numOdd, bool debug = false)
        {
            // New object for generating random numbers
            // https://docs.microsoft.com/en-us/dotnet/api/system.random?f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Random);k(DevLang-csharp)%26rd%3Dtrue&view=netcore-3.1
            Random rnd = new Random(0);

            List<int> lstRandoms = new List<int>();
            for(int i=0; i < capacity; i++)
            {
                lstRandoms.Add(rnd.Next(0, 101));
            }

            if (debug)
                Console.WriteLine($"{DEBUG} The list has been filled!");

            // Uredimo vnose
            lstRandoms.Sort();

            if (debug)
                Console.WriteLine($"{DEBUG} The list has been sorted!");

            // Izpišimo vnose
            foreach (var x in lstRandoms)
                Console.Write($"{x}\t");
            Console.WriteLine();

            if (debug)
                Console.WriteLine($"{DEBUG} The list has been written!");

            // Preverimo, koliko vnosov je lihih
            int tmpOdd = default;
            lstRandoms.ForEach(x => tmpOdd += x % 2 == 1 ? 1 : 0);
            numOdd = tmpOdd;

            if (debug)
                Console.WriteLine($"{DEBUG} Just before the end!");

            return lstRandoms;
        }
    }
}
