using System;
using System.Net.Http.Headers;

namespace PodatkovneStrukture
{
    enum Section
    {
        Insert,
        Delete
    }

    class Program
    {
        static void Main(string[] args)
        {
            Section section = Section.Insert;

            Console.WriteLine();
            switch (section)
            {
                case Section.Insert:
                    {
                        TestSpeed.TestDataStructures(TestAction.Insert);
                    }
                    break;
            }

            Console.ReadLine();
        }
    }
}
