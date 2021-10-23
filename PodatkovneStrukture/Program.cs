using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace PodatkovneStrukture
{
    enum Section
    {
        ICollection,
        IList,
        ISet,
        IDictionary,
        ITest,
        TestSpeed,
        HanoiExample,
    }

    class Program
    {
        static void Main(string[] args)
        {
            Section section = Section.HanoiExample;

            Console.WriteLine();
            switch (section)
            {
                case Section.ICollection:
                case Section.IList:
                case Section.ISet:
                case Section.IDictionary:
                    {
                        ICollectionTest.MethodsOfICollection(section);
                    }
                    break;
                case Section.ITest:
                    {
                        IList<string> testList = new MyList<string>();
                        testList.Add("Luka");
                        testList.Add("Jernej");
                        testList.Add("Borut");
                        Console.WriteLine($"Imamo {testList.Count} elementov!");
                    }
                    break;
                case Section.TestSpeed:
                    {
                        TestAction action = TestAction.Find;

                        TestSpeed.NUMBERS_UP_TO = 101;
                        TestSpeed.TestDataStructures(action);
                        Console.WriteLine();

                        TestSpeed.NUMBERS_UP_TO = 1001;
                        TestSpeed.TestDataStructures(action);
                        Console.WriteLine();

                        TestSpeed.NUMBERS_UP_TO = 10001;
                        TestSpeed.TestDataStructures(action);
                    }
                    break;
                case Section.HanoiExample:
                    {
                        Console.WriteLine("Hanoi example ");
                        HanoiType type = Hanoi.SelectHanoiType();

                        Console.Write("Enter number of discs: ");
                        int k = int.Parse(Console.ReadLine());

                        Console.WriteLine($"Running case: {type} with {k} discs:");

                        int numPegs = 4; // Delali bodo samo s štirimi stolpi

                        Stopwatch sw = Stopwatch.StartNew();
                        Hanoi hanBasic = new Hanoi(k, numPegs, type);
                        int length = hanBasic.ShortestPathForSmallDimension(0, out _);

                        Console.WriteLine();
                        Console.WriteLine($"\n\nDimension: {k}; Steps: {length}; Time: {sw.Elapsed.TotalSeconds}");
                        Console.WriteLine();
                    }
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Končano");
            Console.ReadLine();
        }
    }
}
