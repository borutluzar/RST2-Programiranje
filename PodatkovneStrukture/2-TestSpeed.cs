using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PodatkovneStrukture
{

    public enum TestAction
    {
        Insert,
        Delete,
        Find
    }

    static class TestSpeed
    {
        private const int BOUND = 10000000;
        private const int FINDNUM = 100000;
        public static int NUMBERS_UP_TO = 10001;

        /// <summary>
        /// V tej metodi preizkusimo hitrost vstavljanja mnogo elementov
        /// v različne strukture.
        /// </summary>
        public static void TestDataStructures(TestAction action)
        {
            Stack<int> sklad = new Stack<int>(BOUND);
            Queue<int> vrsta = new Queue<int>(BOUND);
            List<int> seznam = new List<int>(BOUND);
            LinkedList<int> povezanSeznam = new LinkedList<int>();
            SortedSet<int> urejenaMnozica = new SortedSet<int>();
            HashSet<int> zgoscenaTabela = new HashSet<int>(BOUND);

            List<IEnumerable<int>> dataStructures = new List<IEnumerable<int>>()
                {
                    sklad,
                    vrsta,
                    seznam,
                    povezanSeznam,
                    urejenaMnozica,
                    zgoscenaTabela,
                };

            switch (action)
            {
                case TestAction.Insert:
                    {
                        Console.WriteLine($"Čas vstavljanja {BOUND} elementov v: ");
                        foreach (var structure in dataStructures)
                        {
                            Console.WriteLine($"{structure.GetType().Name}: \t {Insert(structure)}");
                        }
                    }
                    break;
                case TestAction.Find:
                    {
                        Console.WriteLine($"Čas iskanja {FINDNUM} elementov v: ");
                        foreach (var structure in dataStructures)
                        {
                            bool allIn = Find(structure, out double time);
                            Console.WriteLine($"{structure.GetType().Name}: \t {time}");
                            Console.WriteLine($"Vsebuje vse: \t {allIn}");
                        }
                    }
                    break;
            }
        }

        private static double Insert(IEnumerable<int> dataStructure)
        {
            // Vstavimo nekaj elementov v vsako izmed struktur in izmerimo čas vstavljanja
            Random rnd = new Random(1); // Dodamo seed, da so rezultati vedno enaki

            DateTime dtStart = DateTime.Now;
            for (int i = 0; i < BOUND; i++)
            {
                int x = rnd.Next(0, NUMBERS_UP_TO);
                if (dataStructure is ICollection<int>)
                    ((ICollection<int>)dataStructure).Add(x);
                else if (dataStructure is Stack<int>)
                    (dataStructure as Stack<int>).Add(x);
                else if (dataStructure is Queue<int>)
                    ((Queue<int>)dataStructure).Add(x);

            }
            DateTime dtEnd = DateTime.Now;

            return (dtEnd - dtStart).TotalSeconds;
        }

        private static bool Find(IEnumerable<int> dataStructure, out double time)
        {
            // Najprej dodamo elemente v strukturo
            Insert(dataStructure);

            // Poiščemo nekaj vrednosti
            Random rnd = new Random(2);
            bool containsAll = true;

            DateTime dtStart = DateTime.Now;
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
            DateTime dtEnd = DateTime.Now;

            time = (dtEnd - dtStart).TotalSeconds;

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
