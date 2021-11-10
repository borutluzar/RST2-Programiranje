using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti.Inheritance
{
    public static class Inheritance
    {
        public static void TestInheritance1()
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
            (child as ParentClass).Property1 = (int)Math.E;

            // Oglejmo si vrednosti lastnosti v oknu Quick Watch
            // (kurzor na izbrani objekt in desni klik)

            Console.WriteLine("Vrednosti objekta child:");
            Console.WriteLine($"\t Property1 = {child.Property1}");
            Console.WriteLine($"\t SquareField = {child.SquareField()}");
            Console.WriteLine($"\t base.Property1 = {((ParentClass)child).Property1}");
            Console.WriteLine();
        }

        public static void TestInheritanceWithCasting()
        {
            // Oglejmo si učinek cast-anja                        
            ChildClass child = new ChildClass(Math.PI);
            ParentClass childAsParent = child;

            // Instanca childAsParent se obnaša kot instanca razreda ParentClass
            Console.WriteLine($"Vrednost funkcije SquareField za child = {child.SquareField()}");
            Console.WriteLine($"Vrednost funkcije SquareField za childAsParent = {childAsParent.SquareField()}");
        }

        public static void TestInheritanceWithOverridenMethod()
        {
            // Oglejmo si učinek cast-anja na povoženo metodo
            Rook rook = new Rook();
            ChessPiece piece = (ChessPiece)rook;

            // Vedno se kliče metoda iz dejanskega razreda instance!
            // Če hočemo imeti dostop do metode v nadrazredu,
            // je v podrazredu ne povozimo, ampak uporabimo določilo new
            Console.WriteLine($"ToString za rook = {rook.ToString()}");
            Console.WriteLine($"ToString za piece = {piece.ToString()}");
        }

        public static void TestInheritanceWithPolymorphisms()
        {
            Player player = new Player();
            player.MyPieces.Add(new Rook());
            player.MyPieces.Add(new Rook());
            player.MyPieces.Add(new King());

            // Izpišimo figure iz seznama
            Console.WriteLine($"Figure igralca player so:");
            player.MyPieces.ForEach(fig => Console.WriteLine($"{fig}\n"));
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Ustvarimo razred, ki vsebuje skupne metode, polja in lastnosti vseh podrazredov
    /// </summary>
    public class ParentClass
    {
        public int Property1 { get; set; }

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
        /// Javna metoda, ki vrača kvadrat vrednosti polja field1
        /// </summary>
        /// <returns>Kvadrat vrednosti field1</returns>
        public double SquareField()
        {
            return this.field1 * this.field1;
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
        /// Podobno pri lastnosti lahko definiramo novo metodo, ki povozi metodo nadrazreda.
        /// </summary>
        new public double SquareField()
        {
            return base.SquareField() * this.field2;
        }
    }


    /// <summary>
    /// Še en primer nadrazreda
    /// </summary>
    public class ChessPiece
    {
        public double ChessWeight { get; protected set; }

        public override string ToString()
        {
            return $"Sem šahovska figura z vrednostjo {this.ChessWeight}.";
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
            this.ChessWeight = chessWeight;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nMoje ime je {this.GetType()}";
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
            this.ChessWeight = chessWeight;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nMoje ime je {this.GetType()}";
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
        public List<ChessPiece> MyPieces { get; }  = new List<ChessPiece>();
    }
}
