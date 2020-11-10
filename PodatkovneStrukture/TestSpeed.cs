using System;
using System.Collections.Generic;
using System.Text;

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
        private const int BOUND = 1000000;

        /// <summary>
        /// V tej metodi preizkusimo hitrost vstavljanja mnogo elementov
        /// v različne strukture.
        /// </summary>
        public static void TestDataStructures(TestAction action)
        {
            Stack<int> sklad = new Stack<int>();
            Queue<int> vrsta = new Queue<int>();
            List<int> seznam = new List<int>();
            LinkedList<int> povezanSeznam = new LinkedList<int>();
            SortedSet<int> urejenaMnozica = new SortedSet<int>();
            HashSet<int> zgoscenaTabela = new HashSet<int>();

            List<IEnumerable<int>> dataStructures = new List<IEnumerable<int>>()
                { 
                    sklad,
                    vrsta,
                    seznam,
                    povezanSeznam,
                    urejenaMnozica,
                    zgoscenaTabela
                };

            Console.WriteLine($"Čas vstavljanja {BOUND} elementov v: ");
            foreach(var structure in dataStructures)
            {
                Console.WriteLine($"{structure.GetType().Name}: \t {Insert(structure)}");
            }
            
            /*Console.WriteLine($"Vrsto: {Insert(vrsta)}");
            Console.WriteLine($"Tabelarični seznam: {Insert(seznam)}");
            Console.WriteLine($"Povezan seznam: {Insert(povezanSeznam)}");
            Console.WriteLine($"Urejena množica: {Insert(urejenaMnozica)}");
            Console.WriteLine($"Zgoščena tabela: {Insert(zgoscenaTabela)}");*/
        }

        private static double Insert(IEnumerable<int> dataStructure)
        {
            // Vstavimo nekaj elementov v vsako izmed struktur in izmerimo čas vstavljanja
            Random rnd = new Random(1); // Dodamo seed, da so rezultati vedno enaki

            DateTime dtStart = DateTime.Now;
            for (int i = 0; i < BOUND; i++)
            {                
                int x = rnd.Next(0, 101);
                if (dataStructure is ICollection<int>)
                    ((ICollection<int>)dataStructure).Add(x);
                else if (dataStructure is Stack<int>)
                    ((Stack<int>)dataStructure).Push(x);
                else if (dataStructure is Queue<int>)
                    ((Queue<int>)dataStructure).Enqueue(x);                

            }
            DateTime dtEnd = DateTime.Now;

            return (dtEnd - dtStart).TotalSeconds;
        }
    }
}
