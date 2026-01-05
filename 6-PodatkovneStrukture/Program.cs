using CommonFunctions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PodatkovneStrukture
{
    enum StructuresSections
    {
        InsertExample = 1,
        ICollection = 2,
        IList = 3,
        ISet = 4,
        IDictionary = 5,
        ITest = 6,
        TestSpeed = 7,
        HanoiExample = 8,
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
                        // 0231 <podatki 1> -> 0 + 2 + 3 + 1 = 6 % 6 = 0
                        // 0111 <podatki 2> -> 0 + 1 + 1 + 1 = 3 % 6 = 3
                        // 0038 <podatki 3> -> 0 + 0 + 3 + 8 = 11 % 6 = 5
                        // 0426 <podatki 4> -> 0 + 4 + 2 + 6 = 12 % 6 = 0

                        // Tabela ima 6 polj
                        // [ <1> | <4> |  | <2> |  | <3> ]

                        int countNum = 5_000_000;
                        int upperBound = 10_000_000;
                        int numAdditional = 10_000;

                        List<int> lstNumbers = new(10 * countNum);
                        HashSet<int> hshNumbers = new(10 * countNum);
                        SortedSet<int> srtNumbers = new();

                        Random rnd = new();

                        // Napolnimo seznam in zgoščeno tabelo s 500 000 števili
                        Stopwatch sw = Stopwatch.StartNew();
                        while (lstNumbers.Count < countNum)
                        {
                            lstNumbers.Add(rnd.Next(upperBound));
                        }
                        Console.WriteLine($"V seznam smo {lstNumbers.Count} števil " +
                            $"vstavili v {sw.Elapsed.TotalSeconds:0.000} sekundah.");

                        sw = Stopwatch.StartNew();
                        while (srtNumbers.Count < countNum)
                        {
                            srtNumbers.Add(rnd.Next(upperBound));
                        }
                        Console.WriteLine($"V urejeno množico smo {srtNumbers.Count} števil " +
                            $"vstavili v {sw.Elapsed.TotalSeconds:0.000} sekundah.");

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
                            $"vstavili v {sw.Elapsed.TotalSeconds:0.000} sekundah.");

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
                            $"trajalo je {sw.Elapsed.TotalSeconds:0.000} sekund.");

                        sw = Stopwatch.StartNew();
                        for (int i = 0; i < numAdditional; i++)
                        {
                            int newNumber = rnd.Next(upperBound);

                            // Urejena množica ima srednjo učinkovitost - log(n)
                            srtNumbers.Add(newNumber);
                        }
                        Console.WriteLine($"V urejeni množici imamo {srtNumbers.Count} vrednosti, " +
                            $"trajalo je {sw.Elapsed.TotalSeconds:0.000} sekund.");

                        sw = Stopwatch.StartNew();
                        for (int i = 0; i < numAdditional; i++)
                        {
                            int newNumber = rnd.Next(upperBound);

                            // HashSet je v tem primeru najhitrejša - O(1)
                            hshNumbers.Add(newNumber);
                        }
                        Console.WriteLine($"V zgoščeni tabeli imamo {hshNumbers.Count} vrednosti, " +
                            $"trajalo je {sw.Elapsed.TotalSeconds:0.000} sekund.");
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
