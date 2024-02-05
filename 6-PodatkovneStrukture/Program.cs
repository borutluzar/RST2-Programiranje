using CommonFunctions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PodatkovneStrukture
{
    enum StructuresSections
    {
        InsertExample = 0,
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
                case StructuresSections.InsertExample:
                    {
                        List<int> lstNumbers = new();
                        Random rnd = new(23);
                        int countNum = 500_000;
                        int upperBound = 10_000_000;

                        // Napolnimo seznam s 500 000 števili
                        while (lstNumbers.Count < countNum)
                        {
                            lstNumbers.Add(rnd.Next(upperBound));
                        }
                        Console.WriteLine($"V seznamu imamo {lstNumbers.Count} vrednosti");

                        // Dodamo 1000 vrednosti,
                        // ampak samo če vrednost še ni bila v seznamu
                        Stopwatch sw = Stopwatch.StartNew();
                        for (int i = 0; i < 10_000; i++)
                        {
                            int newNumber = rnd.Next(upperBound);

                            // Ozko grlo podatkovnih struktur - HashSet je v tem primeru najhitrejša
                            if (!lstNumbers.Contains(newNumber))
                                lstNumbers.Add(newNumber);
                        }
                        Console.WriteLine($"V seznamu imamo {lstNumbers.Count} vrednosti, " +
                            $"trajalo je {sw.Elapsed.TotalSeconds:0.##} sekund.");
                    }
                    break;
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
                        TestSpeed.TestDataStructures(InterfaceFunctions.ChooseSection<TestAction>());
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
                        int length = hanBasic.ShortestPathForSmallDimension(out _);

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
