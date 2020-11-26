using System;

namespace Serializacija
{
    class Program
    {
        enum Section
        {
            Serialize,
            Deserialize,
            Cyclic
        }

        static void Main(string[] args)
        {
            Section section = Section.Cyclic;

            switch (section)
            {
                case Section.Serialize:
                    SerializationBasics.Serialize();
                    break;
                case Section.Deserialize:
                    SerializationBasics.Deserialize();
                    break;
                case Section.Cyclic:
                    CyclicDependencies.CyclicSerialization();
                    break;
            }

            Console.ReadLine();
        }
    }
}
