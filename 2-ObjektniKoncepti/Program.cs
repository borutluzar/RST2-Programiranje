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
            InheritanceKurent = 13,
            InheritanceWithCasting = 5,
            InheritanceWithCastingOnOverriden = 6,
            InheritancePolymorphism = 7,
            InheritanceVirtualMethod = 8,
            Interfaces = 9,
            Abstraction = 10,
            InterfacesImplicitExplicit = 11,
            Extensions = 12
        }

        static void Main()
        {
            switch (InterfaceFunctions.ChooseSection<ObjectsSections>())
            {
                case ObjectsSections.Properties:
                    {
                        Properties.CheckProperties();
                    }
                    break;
                case ObjectsSections.Indexers:
                    {
                        Indexers.TestIndexers();
                    }
                    break;
                case ObjectsSections.BoxingUnboxing:
                    {
                        BoxingUnboxing.TestBoxing();
                    }
                    break;
                case ObjectsSections.Inheritance:
                    {
                        // V namespace-u Inheritance smo naredili nov razdelek, 
                        // zato do ustrezne funkcije pridemo šele preko
                        // ustrezne poti
                        Inheritance.Inheritance.TestInheritance1();
                    }
                    break;
                case ObjectsSections.InheritanceKurent:
                    {
                        // V namespace-u Inheritance smo naredili nov razdelek, 
                        // zato do ustrezne funkcije pridemo šele preko
                        // ustrezne poti
                        Inheritance.Inheritance.ExampleMasks();
                    }
                    break;
                case ObjectsSections.InheritanceWithCasting:
                    {
                        Inheritance.Inheritance.TestInheritanceWithCasting();
                    }
                    break;
                case ObjectsSections.InheritanceWithCastingOnOverriden:
                    {
                        Inheritance.Inheritance.TestInheritanceWithOverridenMethod();
                    }
                    break;
                case ObjectsSections.InheritancePolymorphism:
                    {
                        Inheritance.Inheritance.TestInheritanceWithPolymorphisms();
                    }
                    break;
                case ObjectsSections.InheritanceVirtualMethod:
                    {
                        Inheritance.Inheritance.TestInheritanceWithVirtual();
                    }
                    break;
                case ObjectsSections.Interfaces:
                    {
                        Interfaces.Interfaces.InterfacesExample();
                    }
                    break;
                case ObjectsSections.Abstraction:
                    {
                        Abstraction.Abstraction.TestAbstraction();
                    }
                    break;
                case ObjectsSections.InterfacesImplicitExplicit:
                    {
                        InterfacesImplicitExplicit.ImplicitExplicit.TestImplicitExplicit();
                    }
                    break;
                case ObjectsSections.Extensions:
                    {
                        Extensions.Extensions.TestExtensions();
                    }
                    break;
            }
            Console.ReadLine();
        }
    }

}
