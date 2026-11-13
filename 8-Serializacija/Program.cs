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

}
