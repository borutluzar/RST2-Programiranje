using CommonFunctions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace PodatkovneStrukture
{
    enum StructuresSections
    {
        ICollection = 1,
        IList = 2,
        ISet = 3,
        IDictionary = 4,
        ITest = 5,
        TestSpeed = 6,
        HanoiExample = 7,
    }

    class Program
    {
        static void Main()
        {
            var section = InterfaceFunctions.ChooseSection<StructuresSections>();
            switch (section)
            {
                case StructuresSections.ICollection:
                case StructuresSections.IList:
                case StructuresSections.ISet:
                case StructuresSections.IDictionary:
                    {
                        ICollectionTest.MethodsOfICollection(section);
                    }
                    break;
                case StructuresSections.ITest:
                    {
                        IList<string> testList = new MyList<string>();
                        testList.Add("Luka");
                        testList.Add("Jernej");
                        testList.Add("Borut");
                        Console.WriteLine($"Imamo {testList.Count} elementov!");
                    }
                    break;
                case StructuresSections.TestSpeed:
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
                case StructuresSections.HanoiExample:
                    {
                        Console.WriteLine("Hanoi example ");
                        HanoiType type = Hanoi.SelectHanoiType();

                        Console.Write("Enter number of discs: ");
                        int k = int.Parse(Console.ReadLine());

                        Console.WriteLine($"Running case: {type} with {k} discs:");

                        int numPegs = 4; // Delali bomo samo s štirimi stolpi

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
