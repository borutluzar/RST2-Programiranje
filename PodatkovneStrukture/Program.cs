using System;
using System.Net.Http.Headers;

namespace PodatkovneStrukture
{
    enum Section
    {
        ICollection,
        IList,
        ISet,
        IDictionary,
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
                case Section.TestSpeed:
                    {
                        TestAction action = TestAction.Find;

                        TestSpeed.NUMBERS_UP_TO = 101;
                        TestSpeed.TestDataStructures(action);

                        TestSpeed.NUMBERS_UP_TO = 1001;
                        TestSpeed.TestDataStructures(action);

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

                        Hanoi hanBasic = new Hanoi(k, numPegs, type);
                        int length = hanBasic.ShortestPathForSmallDimension(0, out _);

                        Console.WriteLine();
                        Console.WriteLine("\n\nDimension: " + k + "; Steps: " + length);
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
