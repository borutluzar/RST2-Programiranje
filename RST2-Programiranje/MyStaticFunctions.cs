using System;
using System.Collections.Generic;
using System.Text;

namespace RST2_Programiranje
{
    class MyStaticFunctions
    {
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
    }
}
