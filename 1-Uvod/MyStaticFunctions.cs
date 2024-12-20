﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Uvod
{
    public class MyStaticFunctions
    {
        // Samo primer - boljše je imeti tovrstne flag-e v
        // konfiguracijskih datotekah,
        // ker ni potreben recompile.
        public const int SEED = 0;  // If -1, then do not use seeds

        // Z določilom const definiramo spremenljivke,
        // ki jim ne bomo spreminjali vrednosti.
        private const string DEBUG = "Deb>";

        /// <summary>
        /// Counts all prime numbers up to n
        /// </summary>
        /// <param name="n">An integer up to which we count primes</param>
        /// <returns>Number of primes up to integer n</returns>
        public static int CountPrimes(int n)
        {
            int countPrimes = 0;

            // For each i between 1 and n check if it is a prime number
            for (int i = 2; i <= n; i++)
            {
                // Count number of divisors of the current number i
                int countDivisors = 0;
                for (int j = 2; j <= Math.Sqrt(i); j++)
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
        /// Counts all prime numbers up to n
        /// </summary>
        /// <param name="n">An integer up to which we count primes</param>
        /// <returns>Number of primes up to integer n</returns>
        public static int CountPrimes(int n, out int largest)
        {
            int countPrimes = 0;
            largest = 0; // Nastavimo privzeto vrednost out parametra

            // For each i between 1 and n check if it is a prime number
            for (int i = 2; i <= n; i++)
            {
                // Count number of divisors of the current number i
                int countDivisors = 0;
                for (int j = 2; j <= Math.Sqrt(i); j++)
                {
                    if (i % j == 0)
                        countDivisors++;

                    if (countDivisors > 0)
                        break;
                }

                if (countDivisors == 0)
                {
                    countPrimes++;
                    largest = i;
                }
            }

            return countPrimes;
        }

        /// <summary>
        /// Counts all prime numbers up to n
        /// </summary>
        /// <param name="n">An integer up to which we count primes, at the end</param>
        /// <param name="largest">Number of primes up to integer n</param>
        /// <returns>Number of primes up to integer n</returns>
        public static int CountPrimes(ref int n, out int largest)
        {
            int countPrimes = 0;
            largest = 0; // Nastavimo privzeto vrednost out parametra

            // For each i between 1 and n check if it is a prime number
            for (int i = 2; i <= n; i++)
            {
                // Count number of divisors of the current number i
                int countDivisors = 0;
                for (int j = 2; j <= Math.Sqrt(i); j++)
                {
                    if (i % j == 0)
                        countDivisors++;

                    if (countDivisors > 0)
                        break;
                }

                if (countDivisors == 0)
                {
                    countPrimes++;
                    largest = i;
                }
            }

            // Povečamo vrednost parametra n za 1.
            n++;
            return countPrimes;
        }

        /// <summary>
        /// Counts all prime numbers up to n
        /// </summary>
        /// <param name="n">An integer up to which we count primes</param>
        /// <returns>Number of primes up to integer n and the largest among them</returns>
        public static (int, int) CountPrimesAndFindLargest(int n)
        {
            int countPrimes = 0;
            int max = 0;

            // For each i between 1 and n check if it is a prime number
            for (int i = 2; i <= n; i++)
            {
                // Count number of divisors of the current number i
                int countDivisors = 0;
                for (int j = 2; j <= Math.Sqrt(i); j++)
                {
                    if (i % j == 0)
                        countDivisors++;

                    if (countDivisors > 0)
                        break;
                }

                if (countDivisors == 0)
                {
                    countPrimes++;
                    max = i;
                }
            }

            return (countPrimes, max);
        }

        /// <summary>
        /// Creates a list of integers with random entries
        /// </summary>
        /// <param name="capacity">Number of elements</param>
        /// <returns>List with random entries</returns>
        public static List<int> MakeRandomList(int capacity, out int numOdd, bool debug = false, int seed = 0) // parametra debug in seed sta opcijska, saj imata določeno privzeto vrednost
        {
            // Ustvarimo nov objekt za generiranje naključnih števil
            // Določimo tudi "seed", da bo "naključnost" v naših primerih vedno enaka
            // https://docs.microsoft.com/en-us/dotnet/api/system.random?f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Random);k(DevLang-csharp)%26rd%3Dtrue&view=net-5.0
            Random rnd = SEED > -1 ? new Random(SEED) : new Random();

            // Izbiramo števila med 0 in 100 in jih dodajamo v seznam
            List<int> lstRandoms = new List<int>();
            for (int i = 0; i < capacity; i++)
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
            // To lahko storimo na dva načina:
            lstRandoms.ForEach(x => tmpOdd += x % 2 == 1 ? 1 : 0);
            /*lstRandoms.ForEach(x =>
                {
                    if (x % 2 == 1)
                        tmpOdd++;
                });*/
            numOdd = tmpOdd;

            if (debug)
                Console.WriteLine($"{DEBUG} Just before the end!");

            return lstRandoms;
        }

        /// <summary>
        /// Creates a list of integers with random entries
        /// </summary>
        /// <param name="capacity">Number of elements</param>
        /// <returns>List with random entries</returns>
        public static IEnumerable<int> MakeRandomListWithYield(int capacity)
        {
            // Ustvarimo nov objekt za generiranje naključnih števil
            // Določimo tudi "seed", da bo "naključnost" v naših primerih vedno enaka
            // https://docs.microsoft.com/en-us/dotnet/api/system.random?f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Random);k(DevLang-csharp)%26rd%3Dtrue&view=net-5.0
            Random rnd = SEED > -1 ? new Random(SEED) : new Random();

            // Izbiramo števila med 0 in 100 in jih sproti vračamo
            for (int i = 0; i < capacity; i++)
            {
                Thread.Sleep(500); // Počakamo pol sekunde za učinek čakanja

                // Vračamo vrednosti sproti, eno po eno
                yield return rnd.Next(0, 101);
            }
        }

        /// <summary>
        /// Creates a new file with some dummy entries
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        public static void WriteFile(string fileName)
        {
            // Ime datoteke lahko podamo relativno na pot izvajanja programa
            // ali pa absolutno, npr. C:/Temp/imeDatoteke.txt
            StreamWriter swData = new StreamWriter(fileName);
            
            swData.WriteLine("Moje ime je Borut.");
            swData.Flush(); // Zapišemo trenuten buffer v datoteko

            swData.WriteLine("Rad delam na FIŠ.");
            swData.WriteLine("Ali pa tudi ne.");
            swData.WriteLine("Odvisno od dela.");

            // Ne pozabite!
            swData.Close();
            Console.WriteLine($"Datoteka {fileName} je bila uspešno ustvarjena.");
        }

        public static void WriteInFileWithUsing(string fileName)
        {
            // Ob koncu izvajanja bloka using se pokliče metoda dispose,
            // ki počisti objekt swData.
            // V primeru StreamWriter objekta je Close == Dispose.
            using (StreamWriter swData = new StreamWriter(fileName))
            {
                swData.WriteLine("Uporaba usinga");
                // Enako kot zgoraj. Paziti na \r v drugih OS!
                swData.Write("Uporaba usinga\r\n");
                swData.Close();
            }
        }

        /// <summary>
        /// Reads the given file and counts its lines.
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <returns>Number of lines in the file</returns>
        public static int ReadFile(string fileName)
        {
            StreamReader srData = new StreamReader(fileName);

            int countLines = 0;
            // Beremo datoteko, dokler ne pridemo do konca
            while (!srData.EndOfStream)
            {
                string line = srData.ReadLine();
                Console.WriteLine(line);
                countLines++;
            }
            srData.Close();

            return countLines;
        }

        /// <summary>
        /// Reads the given file and writes its properties in a special object
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <returns>Object with file's properties</returns>
        public static FileData ReadFile2(string fileName)
        {
            StreamReader srData = new StreamReader(fileName);

            int countLines = 0;
            int numChars = 0;
            string delicateWord = "Jurij";

            // Dodaten podatek, za katerega želimo, da ga funkcija vrne
            bool checkDelicate = default;
            while (!srData.EndOfStream)
            {
                string line = srData.ReadLine();
                countLines++;
                numChars += line.Length;

                if (line.Contains(delicateWord))
                    checkDelicate = true;
            }
            srData.Close();

            // Ustvarimo nov objekt
            FileData fd = new FileData(checkDelicate)
            {
                NumberOfLines = countLines,
                NumberOfCharacters = numChars,
                //ContainsSensitiveInfo = checkDelicate
            };

            return fd;
        }
    }
}
