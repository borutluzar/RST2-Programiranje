using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace PodatkovneStrukture
{

    public enum TestAction
    {
        Insert = 1,        
        Find = 2
    }

    /// <summary>
    /// Operacije nad podatkovnimi strukturami v C# imajo nasledje časovne zahtevnosti:
    /// https://learn.microsoft.com/en-us/dotnet/standard/collections/
    /// </summary>
    static class TestSpeed
    {
        private const int BOUND = 10_000_000;
        private const int FINDNUM = 10_000;
        public static int NUMBERS_UP_TO = 100_001;

        public static void TestDataStructures(TestAction action)
        {
            Console.WriteLine($"Entered numbers up to {TestSpeed.NUMBERS_UP_TO}");
            TestSpeed.CreateTests(action, true);
        }

        /// <summary>
        /// V tej metodi preizkusimo hitrost vstavljanja mnogo elementov
        /// v različne strukture.
        /// </summary>
        public static void CreateTests(TestAction action, bool allDistinct = false)
        {
            Stack<int> sklad = new Stack<int>();
            Queue<int> vrsta = new Queue<int>();
            List<int> seznam = new List<int>();
            LinkedList<int> povezanSeznam = new LinkedList<int>();
            SortedSet<int> urejenaMnozica = new SortedSet<int>();
            HashSet<int> zgoscenaTabela = new HashSet<int>();

            List<IReadOnlyCollection<int>> dataStructures = new List<IReadOnlyCollection<int>>()
                {                    
                    vrsta,
                    seznam,
                    povezanSeznam,
                    urejenaMnozica,
                    zgoscenaTabela,
                    sklad,
                };

            switch (action)
            {
                case TestAction.Insert:
                    {
                        Console.WriteLine($"Čas vstavljanja {BOUND} elementov v: ");
                        foreach (var structure in dataStructures)
                        {
                            Insert(structure, allDistinct);
                        }
                    }
                    break;
                case TestAction.Find:
                    {
                        Console.WriteLine($"Čas iskanja {FINDNUM} elementov v: \n");
                        foreach (var structure in dataStructures)
                        {
                            Find(structure);
                        }
                    }
                    break;
            }
        }

        private static void Insert(IReadOnlyCollection<int> dataStructure, bool allDistinct = false)
        {
            // Vstavimo nekaj elementov v vsako izmed struktur in izmerimo čas vstavljanja
            Random rnd = new Random(1); // Dodamo seed, da so rezultati vedno enaki

            Stopwatch swTimer = Stopwatch.StartNew();
            for (int i = 0; i < BOUND; i++)
            {
                int x = i;
                if (!allDistinct) // Insert random numbers unless specified otherwise
                    x = rnd.Next(0, NUMBERS_UP_TO);
                if (dataStructure is ICollection<int>)
                    ((ICollection<int>)dataStructure).Add(x);
                else if (dataStructure is Stack<int>)
                    (dataStructure as Stack<int>).Add(x);
                else if (dataStructure is Queue<int>)
                    ((Queue<int>)dataStructure).Add(x);

            }
            Console.WriteLine($"> Čas vstavljanja v {dataStructure.GetType().Name}: \t {swTimer.Elapsed.TotalSeconds}");
            Console.WriteLine($"> Struktura vsebuje {dataStructure.Count} elementov");
        }

        private static bool Find(IReadOnlyCollection<int> dataStructure)
        {
            // Najprej dodamo elemente v strukturo
            Console.WriteLine($"> Vstavljanje elementov");
            Insert(dataStructure, true);

            Console.WriteLine($"> Iskanje {FINDNUM} elementov");
            // Poiščemo nekaj vrednosti
            Random rnd = new Random(2);
            bool containsAll = true;

            Stopwatch swTimer = Stopwatch.StartNew();
            for (int i = 0; i < FINDNUM; i++)
            {
                int x = rnd.Next(0, NUMBERS_UP_TO);

                if (dataStructure is ICollection<int>)
                {
                    if (!((ICollection<int>)dataStructure).Contains(x))
                        containsAll = false;
                }
                else if (dataStructure is Stack<int>)
                {
                    if (!((Stack<int>)dataStructure).Contains(x))
                        containsAll = false;
                }
                else if (dataStructure is Queue<int>)
                {
                    if (!((Queue<int>)dataStructure).Contains(x))
                        containsAll = false;
                }
            }
            Console.WriteLine($"> Čas za {dataStructure.GetType().Name}: \t {swTimer.Elapsed.TotalSeconds}\n");
            return containsAll;
        }
    }

    public static class CollectionExtensions
    {
        public static void Add<T>(this Stack<T> stack, T val)
        {
            stack.Push(val);
        }

        public static void Add<T>(this Queue<T> queue, T val)
        {
            queue.Enqueue(val);
        }
    }
}
