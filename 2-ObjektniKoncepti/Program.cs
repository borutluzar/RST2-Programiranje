using CommonFunctions;
using System;

namespace ObjektniKoncepti
{
    class Program
    {
        /// <summary>
        /// Enumeracija s primeri drugega poglavja.
        /// </summary>
        enum ObjectsSections
        {
            Properties = 1,
            Indexers = 2,
            BoxingUnboxing = 3,
            Inheritance = 4,
            InheritanceMasksExample = 5,
            InheritanceWithCasting = 6,
            InheritanceWithCastingOnOverriden = 7,
            InheritancePolymorphism = 8,
            InheritanceVirtualMethod = 9,
            Abstraction = 10,
            Interfaces = 11,
            InterfacesExampleMetadata = 12,
            InterfacesImplicitExplicit = 13,
            Extensions = 14
        }

        static void Main()
        {
            switch (InterfaceFunctions.ChooseSection<ObjectsSections>())
            {
                case ObjectsSections.Properties:
                    {
                        Properties.ExampleProperties();
                    }
                    break;
                case ObjectsSections.Indexers:
                    {
                        Indexers.ExampleIndexers();
                    }
                    break;
                case ObjectsSections.BoxingUnboxing:
                    {
                        BoxingUnboxing.ExampleBoxing();
                    }
                    break;
                case ObjectsSections.Inheritance:
                    {
                        // V namespace-u Inheritance smo naredili nov razdelek, 
                        // zato do ustrezne funkcije pridemo šele preko
                        // ustrezne poti
                        Inheritance.Inheritance.ExampleInheritance();
                    }
                    break;
                case ObjectsSections.InheritanceMasksExample:
                    {
                        // Primer dedovanja z maškarami in kurenti
                        Inheritance.Inheritance.ExampleMasks();
                    }
                    break;
                case ObjectsSections.InheritanceWithCasting:
                    {
                        // Primer dedovanja, ko na isto instanco
                        // kaže še instanca tipa nadrazreda
                        Inheritance.Inheritance.ExampleInheritanceWithCasting();
                    }
                    break;
                case ObjectsSections.InheritanceWithCastingOnOverriden:
                    {
                        Inheritance.Inheritance.ExampleInheritanceWithOverridenMethod();
                    }
                    break;
                case ObjectsSections.InheritancePolymorphism:
                    {
                        Inheritance.Inheritance.ExampleInheritanceWithPolymorphisms();
                    }
                    break;
                case ObjectsSections.InheritanceVirtualMethod:
                    {
                        Inheritance.Inheritance.ExampleInheritanceWithVirtual();
                    }
                    break;

                case ObjectsSections.Abstraction:
                    {
                        Abstraction.Abstraction.ExampleAbstraction();
                    }
                    break;

                case ObjectsSections.Interfaces:
                    {
                        Interfaces.Interfaces.InterfacesExample();
                    }
                    break;
                case ObjectsSections.InterfacesExampleMetadata:
                    {
                        Interfaces.Interfaces.ExampleMetaData();
                    }
                    break;
                
                case ObjectsSections.InterfacesImplicitExplicit:
                    {
                        InterfacesImplicitExplicit.ImplicitExplicit.ExampleImplicitExplicit();
                    }
                    break;
                case ObjectsSections.Extensions:
                    {
                        Extensions.Extensions.ExampleExtensions();
                    }
                    break;
            }
            Console.ReadLine();
        }
    }

}
