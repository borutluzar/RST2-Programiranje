using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti.Inheritance
{
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

        private readonly int field2;

        /// <summary>
        /// Lastnost Property1 je že definirana v nadrazredu, 
        /// zato eksplicitno navedemo, da jo želimo predefinirati
        /// z rezervirano besedo 'new' (koda se prevede tudi brez nje)
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
        public Rook()
        {
            this.ChessWeight = 4.9;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nMoje ime je {this.GetType()}";
        }
    }
}
