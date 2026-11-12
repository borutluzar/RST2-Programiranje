using CommonFunctions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Serializacija
{
    class Program
    {
        enum SerializationSections
        {
            Serialize,
            Deserialize,
            Cyclic
        }

        static void Main(string[] args)
        {
            Position p1 = new Position() { X = 1, Y = 2, Z = 0 };
            Position p2 = new Position() { X = 1, Y = 2, Z = 100 };

            p1.Equals(p2 as object);

            Console.WriteLine($"p1: {p1.GetHashCode()}; p2: {p2.GetHashCode()}");
            Dictionary<Position, Position> dicPos = new();
            dicPos.Add(p1, p1);

            string pal1 = "ribarežeracirep";
            string pal2 = "pericarežeracirep";
            string pal3 = "ana";
            string pal4 = "atanimalaminata";

            Console.WriteLine($"{pal1} {(pal1.IsPalindrom() ? "je" : "ni")} palindrom!");
            Console.WriteLine($"{pal2} {(pal2.IsPalindrom() ? "je" : "ni")} palindrom!");
            Console.WriteLine($"{pal3} {(pal3.IsPalindrom() ? "je" : "ni")} palindrom!");
            Console.WriteLine($"{pal4} {(pal4.IsPalindrom() ? "je" : "ni")} palindrom!");


            switch (InterfaceFunctions.ChooseSection<SerializationSections>())
            {
                case SerializationSections.Serialize:
                    SerializationBasics.Serialize();
                    break;
                case SerializationSections.Deserialize:
                    SerializationBasics.Deserialize();
                    break;
                case SerializationSections.Cyclic:
                    CyclicDependencies.CyclicSerialization();
                    break;
            }

            Console.ReadLine();
        }
    }

    //3.6.4
    public struct Position : IEquatable<Position>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public bool Equals(Position other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position other)
                return this.Equals(other);
            else return false;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode();
        }
    }

    // 3.8.3
    public static class Extensions
    {
        public static bool IsPalindrom(this string str)
        {
            for (int i = 0; i < str.Length / 2; i++)
            {
                if (str[i] != str[str.Length - 1 - i])
                    return false;
            }
            return true;
        }
    }
}
