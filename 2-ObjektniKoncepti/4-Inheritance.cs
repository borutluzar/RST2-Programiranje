using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti.Inheritance
{
    public static class Inheritance
    {
        public static void ExampleInheritance()
        {
            ParentClass parent = new ParentClass(Math.PI);
            parent.Property1 = (int)(2 * Math.PI); // Cast vzame prvi objekt z desne

            Console.WriteLine();
            Console.WriteLine("Vrednosti objekta parent:");
            Console.WriteLine($"\t Property1 = {parent.Property1}");
            Console.WriteLine($"\t SquareField = {parent.SquareField()}");
            Console.WriteLine();

            ChildClass child = new ChildClass(Math.PI);
            child.Property1 = (int)(3 * Math.PI);
            (child as ParentClass).Property1 = (int)Math.E; // Na nastavljanje te lastnosti s castom gledamo kot na dodatno delo,
                                                            // ker smo se odločili za vztrajanje pri lastnostih z istim imenom.
                                                            // Tega se v praksi izogibamo.

            child.PropertyParent = 7; // Lastnost z drugim imenom iz prednika vidimo tudi v podrazredu.

            // Oglejmo si vrednosti lastnosti v oknu Quick Watch
            // (kurzor na izbrani objekt in desni klik)

            Console.WriteLine("Vrednosti objekta child:");
            Console.WriteLine($"\t Property1 = {child.Property1}");
            Console.WriteLine($"\t SquareField = {child.SquareField()}");
            Console.WriteLine($"\t base.Property1 = {((ParentClass)child).Property1}");
            Console.WriteLine();
        }

        public static void ExampleInheritanceWithCasting()
        {
            // Oglejmo si učinek cast-anja        
            ChildClass child = new ChildClass(Math.PI);
            ParentClass childAsParent = child;
            ParentClass childAsParent2 = new ChildClass(4);
            
            // Obratno seveda ne moremo storiti. Zakaj?
            //ChildClass child2 = new ParentClass(1);

            // Nastavimo vrednosti lastnostim
            child.Property1 = 11;
            child.PropertyParent = 3;
            childAsParent.Property1 = 12;

            // Preverimo, če je instanca ustreznega tipa
            if (childAsParent2 is ChildClass)
            {
                //childAsParent2.ChildMethod();
                ((ChildClass)childAsParent2).ChildMethod();
            }            

            // Instanca childAsParent se obnaša kot instanca razreda ParentClass,
            // vendar le, če metode ne "povozimo" v podrazredu!
            Console.WriteLine($"Vrednost funkcije SquareField za child = {child.SquareField()}");
            Console.WriteLine($"Vrednost funkcije SquareField za childAsParent = {childAsParent.SquareField()}");

            // Če metodo "povozimo" v podrazredu, potem se definicija metode poišče tako
            // globoko po drevesu dedovanja, dokler ne pridemo kar najbližje osnovnemu razredu instance!
            Console.WriteLine($"Vrednost funkcije SquareRootField za child = {child.SquareRootField()}");
            Console.WriteLine($"Vrednost funkcije SquareRootField za childAsParent = {childAsParent.SquareRootField()}");
        }

        public static void ExampleMasks() 
        {
            // Še en primer dedovanja, ko imamo nadrazred za splošno masko
            // in podrazrede za specifične maske
            Kurent kurent1 = new Kurent(48.3, 1610);

            Console.WriteLine($"{kurent1}");
        }

        public static void AnalyzeInstance(ParentClass parent)
        {
            Console.WriteLine($"Tip: {parent.GetType()}");
        }

        public static void ExampleInheritanceWithOverridenMethod()
        {
            ChessPiece generalPiece = new ChessPiece();
            Console.WriteLine($"ToString za {nameof(generalPiece)} = {generalPiece.ToString()}");

            // Oglejmo si učinek cast-anja na povoženo metodo            
            Rook rook = new Rook();
            ChessPiece piece = rook;

            // Vedno se kliče metoda iz dejanskega razreda instance!
            // Če hočemo imeti dostop do metode v nadrazredu,
            // je v podrazredu ne povozimo, ampak uporabimo določilo new (glejte prejšnji primer)
            Console.WriteLine($"ToString za {nameof(rook)} = {rook.ToString()}");
            Console.WriteLine($"ToString za {nameof(piece)} = {piece.ToString()}");
        }

        /// <summary>
        /// Polimorfizmi nam omogočajo različno obnašanje objektov
        /// "istega" tipa.
        /// </summary>
        public static void ExampleInheritanceWithPolymorphisms()
        {
            Player player = new Player();
            
            Rook rook1 = new Rook();
            player.MyPieces.Add(rook1);
            Rook rook2 = new Rook();
            player.MyPieces.Add(rook2);

            King king = new King();
            player.MyPieces.Add(king);

            // Izpišimo figure iz seznama
            Console.WriteLine($"Figure igralca player so:");
            // Za vsako figuro fig se kliče njena metoda ToString
            player.MyPieces.ForEach(fig => Console.WriteLine($"{fig}\n"));
            Console.WriteLine();
        }

        /// <summary>
        /// Naredimo še primer z implementacijo virtualne metode
        /// in njenimi reimplementacijami
        /// </summary>
        public static void ExampleInheritanceWithVirtual()
        {
            List<Animal> lstAnimals = new List<Animal>()
            {
                new Animal(),
                new Elephant(),
                new Ostrich(),
                new Elephant()
            };

            // Izpišimo oglašanje živali iz seznama
            Console.WriteLine($"Oglašanje živali:");
            // Metoda MakeSound se pokliče iz razreda, katerega instanca je anim
            lstAnimals.ForEach(anim => Console.WriteLine($"{anim.MakeSound()}\n"));
            Console.WriteLine();
        }
    }

    internal class Mask
    {
        int FirstAppearanceInYear { get; set; }

        public Mask(int firstAppearance)
        {
            this.FirstAppearanceInYear = firstAppearance;
        }
    }

    internal class Kurent : Mask
    {
        double TongueLength { get; set; }

        public Kurent(double tongueLength, int firstAppearance) : base(firstAppearance)
        {
            this.TongueLength = tongueLength;
        }
    }


    /// <summary>
    /// Ustvarimo razred, ki vsebuje skupne metode, polja in lastnosti vseh podrazredov
    /// </summary>
    public class ParentClass
    {
        public int Property1 { get; set; }

        public int PropertyParent { get; set; }

        /// <summary>
        /// Polje nastavljamo le v konstruktorju, zato ga označimo kot 'readonly'
        /// </summary>
        private readonly double field1;

        /// <summary>
        /// Konstruktor nadrazreda
        /// </summary>
        /// <param name="f">Vrednost, ki jo nastavimo polju field1</param>
        public ParentClass(double f)
        {
            this.field1 = f;
        }

        /// <summary>
        /// Metoda, ki vrača kvadrat vrednosti polja field1
        /// </summary>
        public double SquareField()
        {
            return this.field1 * this.field1;
        }

        /// <summary>
        /// Metoda, ki vrača kvadratni koren vrednosti polja field1
        /// </summary>
        public virtual double SquareRootField()
        {
            return Math.Sqrt(this.field1);
        }
    }

    /// <summary>
    /// Razred, ki deduje od razreda ParentClass
    /// </summary>
    public class ChildClass : ParentClass
    {
        /// <summary>
        /// V nadrazredu smo definirali samo konstruktor z enim parametrom, 
        /// zato moramo ta parameter podati tudi v vsakem podrazredu
        /// </summary>
        /// <param name="g">Parameter f iz konstruktorja nadrazreda</param>
        public ChildClass(double g) : base(g)
        {
            this.field2 = (int)g;
        }

        // Polje ima določilo readonly,
        // z njim zagotovimo, da se mu vrednost lahko spremeni
        // le pri deklaraciji oziroma v konstruktorju.
        public readonly int field2;

        /// <summary>
        /// Lastnost Property1 je že definirana v nadrazredu, 
        /// zato eksplicitno navedemo, da jo želimo predefinirati
        /// z rezervirano besedo 'new' (koda se prevede tudi brez nje)
        /// 
        /// Z določilom new zagotovimo, da se med izvajanjem kliče metoda iz razreda, 
        /// katerega tip je določen ob prevajanju kode
        /// </summary>
        new public int Property1 { get; set; }

        /// <summary>
        /// Podobno pri lastnosti lahko definiramo NOVO metodo, ki ima enako ime kot metoda v nadrazredu
        /// </summary>
        new public double SquareField()
        {
            // Kvadrat še dodatno pomnožimo
            return base.SquareField() * this.field2;
        }

        /// <summary>
        /// Lahko pa metodo iz nadrazreda povozimo - jo predefiniramo za potrebe podrazreda
        /// </summary>
        public override double SquareRootField()
        {
            return base.SquareRootField() * this.field2;
        }

        public bool ChildMethod()
        {
            Console.WriteLine("This method is defined only in the child class");
            return true;
        }
    }


    /// <summary>
    /// Še en primer nadrazreda
    /// </summary>
    public class ChessPiece
    {
        public double Weight { get; protected set; }

        public override string ToString()
        {
            //return base.ToString();
            return $"Sem šahovska figura z vrednostjo {this.Weight}.";
        }
    }

    /// <summary>
    /// Dedujemo lahko samo od enega razreda - linija prednikov je linearna!
    /// Vsi objekti dedujejo razred Object (zato tudi imamo vedno na voljo funkcijo ToString())
    /// </summary>
    public class Rook : ChessPiece
    {
        private const double chessWeight = 4.9;

        public Rook()
        {
            this.Weight = chessWeight;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nMoje ime je {this.GetType().Name}";
        }
    }

    /// <summary>
    /// Ustvarimo razred za še eno figuro
    /// </summary>
    public class King : ChessPiece
    {
        private const double chessWeight = double.PositiveInfinity;

        public King()
        {
            this.Weight = chessWeight;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nMoje ime je {this.GetType().Name}";
        }
    }

    /// <summary>
    /// Arh, Q22
    /// Na tem primeru si bomo ogledali koncept polimorfizma
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Lastnost, ki vsebuje trenutne figure igralca
        /// </summary>
        public List<ChessPiece> MyPieces { get; } = new List<ChessPiece>();
    }



    /// <summary>
    /// Naredimo razred z virtualno metodo, ki jo v podrazredih
    /// re-implementirajmo.
    /// </summary>
    public class Animal
    {
        public Animal() { }
        public virtual string Name { get; }
        public virtual string MakeSound() { return "Mu!"; }
        //public virtual string MakeNoise() => "Mu!";
    }

    public class Elephant : Animal
    {
        public Elephant() { }
        public override string Name { get; }
        public override string MakeSound() => "Trara!";
    }

    /// <summary>
    /// Z določilom sealed povemo, da je razred zadnji v vrsti dedovanja,
    /// se pravi, da noben razred ne more dedovati iz njega.
    /// </summary>
    public sealed class Ostrich : Animal
    {
        public Ostrich() { }

        public override string MakeSound() => "Čiv!";
    }
}
