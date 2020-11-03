using System;

namespace ObjektniKoncepti
{
    class Program
    {
        enum Section
        {
            Properties,
            Indexers,
            BoxingUnboxing,
            Inheritance,
            Inheritance2,
            Inheritance3
        }

        static void Main(string[] args)
        {
            Section section = Section.Inheritance3;

            switch (section)
            {
                case Section.Properties:
                    {
                        // Ustvarimo novo instanco objekta
                        PropertiesExample pe = new PropertiesExample();
                        // Vrednost polja lahko beremo in nastavljamo brez omejitev
                        uint x = pe.field;
                        pe.field = 22;

                        Console.WriteLine();
                        Console.WriteLine($"Vrednost x={x}, vrednost polja je {pe.field}");

                        //pe.Property = 2;
                        Console.WriteLine();
                        Console.WriteLine($"Vrednost Property={pe.Property}");

                        //pe.AutoImplementedProperty = 14;
                        Console.WriteLine();
                        Console.WriteLine($"Vrednost AutoImplementedProperty={pe.AutoImplementedProperty}");
                        pe.MethodExample();
                        Console.WriteLine($"Vrednost po izvedbi metode AutoImplementedProperty={pe.AutoImplementedProperty}");

                        //pe.ReadOnlyAutoImplementedProperty = 122;
                        Console.WriteLine();
                        Console.WriteLine($"Vrednost ReadOnlyAutoImplementedProperty={pe.ReadOnlyAutoImplementedProperty}");
                    }
                    break;
                case Section.Indexers:
                    {
                        IndexerExample ie = new IndexerExample();
                        ie["Jabolko"] = 12.0;
                        ie["Hruška"] = 17.0;
                        Console.WriteLine($"Cena jabolka={ie["Jabolko"]}, hruška={ie["Hruška"]}");
                    }
                    break;
                case Section.BoxingUnboxing:
                    {
                        BoxingUnboxing.Test();
                    }
                    break;
                case Section.Inheritance:
                    {
                        Inheritance.ParentClass parent = new Inheritance.ParentClass(Math.PI);
                        parent.Property1 = (int)(2 * Math.PI); // Cast vzame prvi objekt z desne

                        Console.WriteLine("Vrednosti objekta parent:");
                        Console.WriteLine($"\t Property1 = {parent.Property1}");
                        Console.WriteLine($"\t SquareField = {parent.SquareField()}");
                        Console.WriteLine();

                        Inheritance.ChildClass child = new Inheritance.ChildClass(Math.PI);
                        child.Property1 = (int)(3 * Math.PI); // Cast vzame prvi objekt z desne

                        Console.WriteLine("Vrednosti objekta child:");
                        Console.WriteLine($"\t Property1 = {child.Property1}");
                        Console.WriteLine($"\t SquareField = {child.SquareField()}");
                        Console.WriteLine();
                    }
                    break;
                case Section.Inheritance2:
                    {
                        // Oglejmo si učinek cast-anja                        
                        Inheritance.ChildClass child = new Inheritance.ChildClass(Math.PI);
                        Inheritance.ParentClass childAsParent = child;

                        Console.WriteLine($"Vrednost funkcije SquareField za child = {child.SquareField()}");
                        Console.WriteLine($"Vrednost funkcije SquareField za childAsParent = {childAsParent.SquareField()}");
                    }
                    break;
                case Section.Inheritance3:
                    {
                        // Oglejmo si učinek cast-anja na povoženo metodo
                        Inheritance.Rook rook = new Inheritance.Rook();
                        Inheritance.ChessPiece piece = (Inheritance.ChessPiece)rook;

                        // Vedno se kliče metode iz dejanskega razreda instance
                        Console.WriteLine($"ToString za rook = {rook.ToString()}");
                        Console.WriteLine($"ToString za piece = {piece.ToString()}");
                    }
                    break;
            }
            Console.ReadLine();
        }
    }

    }
