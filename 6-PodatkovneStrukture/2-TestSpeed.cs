using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private const int FINDNUM = 1_000;
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

                        // Opazimo, da imata pri iskanju vrsta in sklad bistveno različna časa izvajanja,
                        // čeprav je iskanje pri obeh linearno. 
                        // Razlog je v implementaciji funkcije Contains.

                        // Sklad ima v .Net naslednjo:
                        /*
                            public bool Contains(T item)
                            {
                                // Compare items using the default equality comparer

                                // PERF: Internally Array.LastIndexOf calls EqualityComparer<T>.Default.LastIndexOf, which
                                // is specialized for different types. This boosts performance since instead of making a
                                // virtual method call each iteration of the loop,via EqualityComparer<T>.Default.Equals, we
                                // only make one virtual call to EqualityComparer.LastIndexOf.

                                return _size != 0 && Array.LastIndexOf(_array, item, _size - 1) >= 0;
                            }
                         */

                        // Vrsta pa:
                        /*
                            public bool Contains(T item)
                            {
                                if (_size == 0)
                                {
                                    return false;
                                }

                                if (_head < _tail)
                                {
                                    return Array.IndexOf(_array, item, _head, _size) >= 0;
                                }

                                // We've wrapped around. Check both partitions, the least recently enqueued first.
                                return
                                    Array.IndexOf(_array, item, _head, _array.Length - _head) >= 0 ||
                                    Array.IndexOf(_array, item, 0, _tail) >= 0;
                            }
                        */

                        // Obe funkciji sta zelo podobni, sprehodita se po tabelah, ki jih uporabljata sklad in vrsta,
                        // in preverjata, če v njih obstaja iskani element.
                        // Pri skladu se elemente išče z zadnjega konca, pri vrsti pa iz sprednjega.
                        // Pri enakomerni razporeditvi elementov, bi morali biti iskanji v povprečju enaki,
                        // zatakne se, kadar se elementi v strukturah ponavljajo in vrednosti prihajajo iz intervala
                        // manjšega od števila elementov.
                        // V našem primeru imamo BOUND števil, vrednosti pa so vse med 0 in BOUND.
                        // Če iščemo vrednosti iz intervala od 0 do NUMBERS_UP_TO, bodo te na začetku vrste
                        // in hitro najdene, pri skladu pa bodo na koncu.
                        
                        // Sicer pa je v splošnem vrsta 2x do 4x hitrejša (konstanta pri O(n) je manjša)
                        // zaradi implementacije iskanja ob prevajanju v strojno kodo (v kar se ne bomo podrobneje poglabljali).
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
                    x = rnd.Next(0, BOUND);
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
            Insert(dataStructure, false);

            Console.WriteLine($"> Iskanje {FINDNUM} elementov");
            // Poiščemo nekaj vrednosti
            Random rnd = new Random();
            bool containsAll = true;

            Stopwatch swTimer = Stopwatch.StartNew();
            for (int i = 0; i < FINDNUM; i++)
            {
                //int x = rnd.Next(0, BOUND);
                int x = rnd.Next(0, NUMBERS_UP_TO);
                //int x = rnd.Next(BOUND - NUMBERS_UP_TO, BOUND);

                if (dataStructure.Contains(x))
                    containsAll = false; 
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
