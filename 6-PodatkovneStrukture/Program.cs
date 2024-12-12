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
                        HashSet<int> hshNumbers = new();
                        SortedSet<int> srtNumbers = new();

                        Random rnd = new(3);
                        int countNum = 500_000;
                        int upperBound = 10_000_000;
                        int numAdditional = 200_000;

                        // Napolnimo seznam in zgoščeno tabelo s 500 000 števili
                        Stopwatch sw = Stopwatch.StartNew();
                        while (lstNumbers.Count < countNum)
                        {
                            lstNumbers.Add(rnd.Next(upperBound));
                        }
                        Console.WriteLine($"V seznam smo {lstNumbers.Count} števil " +
                            $"vstavili v {sw.Elapsed.TotalSeconds:0.##} sekundah.");

                        sw = Stopwatch.StartNew();
                        while (srtNumbers.Count < countNum)
                        {
                            srtNumbers.Add(rnd.Next(upperBound));
                        }
                        Console.WriteLine($"V urejeno množico smo {srtNumbers.Count} števil " +
                            $"vstavili v {sw.Elapsed.TotalSeconds:0.##} sekundah.");

                        sw = Stopwatch.StartNew();
                        while (hshNumbers.Count < countNum)
                        {
                            hshNumbers.Add(rnd.Next(upperBound));
                        }
                        /*for (int i = 0; i < countNum; i++)
                        {
                            hshNumbers.Add(rnd.Next(upperBound));
                        }*/
                        Console.WriteLine($"V zgoščeno tabelo smo {hshNumbers.Count} števil " +
                            $"vstavili v {sw.Elapsed.TotalSeconds:0.##} sekundah.");

                        // Dodamo dodatne vrednosti,
                        // ampak samo če vrednost še ni bila v seznamu.
                        sw = Stopwatch.StartNew();
                        for (int i = 0; i < numAdditional; i++)
                        {
                            int newNumber = rnd.Next(upperBound);

                            // Ozko grlo podatkovnih struktur - HashSet je v tem primeru najhitrejša
                            if (!lstNumbers.Contains(newNumber)) // linearno iskanje
                                lstNumbers.Add(newNumber);
                        }
                        Console.WriteLine($"V seznamu imamo {lstNumbers.Count} vrednosti, " +
                            $"trajalo je {sw.Elapsed.TotalSeconds:0.##} sekund.");

                        sw = Stopwatch.StartNew();
                        for (int i = 0; i < numAdditional; i++)
                        {
                            int newNumber = rnd.Next(upperBound);

                            // Urejena množica ima srednjo učinkovitost - log(n)
                            srtNumbers.Add(newNumber);
                        }
                        Console.WriteLine($"V urejeni množici imamo {srtNumbers.Count} vrednosti, " +
                            $"trajalo je {sw.Elapsed.TotalSeconds:0.##} sekund.");

                        sw = Stopwatch.StartNew();
                        for (int i = 0; i < numAdditional; i++)
                        {
                            int newNumber = rnd.Next(upperBound);

                            // HashSet je v tem primeru najhitrejša - O(1)
                            hshNumbers.Add(newNumber);
                        }
                        Console.WriteLine($"V zgoščeni tabeli imamo {hshNumbers.Count} vrednosti, " +
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
