using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelAndAsync
{
    sealed class DataForParallel
    {
        private List<int> primeCandidates;
        //private const int BOUND = 5000000;
        private const int BOUND = 10_000_000;
        //private const int BOUND = 20000000;

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

    public static class CommonFunctions 
    { 
        public static bool IsPrime(int i)
        {
            for (int j = 2; j <= (int)Math.Sqrt(i); j++)
            {
                if (i % j == 0)
                    return false;
            }
            return true;
        }
    }
}
