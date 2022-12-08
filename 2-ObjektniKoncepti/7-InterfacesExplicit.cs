using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti.InterfacesImplicitExplicit
{
    public static class ImplicitExplicit
    {
        public static void TestImplicitExplicit()
        {
            ChessBoardField fieldStart = new ChessBoardField() { X = 1, Y = 1 };
            ChessBoardField fieldEnd = new ChessBoardField() { X = 2, Y = 2 };

            ChessPiece rook = new Rook(fieldStart);
            Console.WriteLine();
            Console.WriteLine($"Trenutna pozicija figure rook je {rook.Position}");


            IPiece rook2 = rook as IPiece;
            Console.WriteLine();
            Console.WriteLine("Eksplicitna implementacija metode Promote iz IPiece");
            rook2.Promote(new Rook(rook.Position));

            Console.WriteLine();
            Console.WriteLine("Implementacija metode Promote iz razreda");
            Console.WriteLine();
            Console.WriteLine("Klic na objektu rook: ");
            rook.Promote(new Rook(rook.Position));

            Console.WriteLine();
            Console.WriteLine("Klic na objektu rook2: ");
            rook2.Promote(new Rook(rook.Position));

            Console.WriteLine();
            Console.WriteLine("Klic na objektu tipa ICareerObject: ");
            ((ICareerObject)rook).Promote(new Rook(rook.Position));

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Arh, Q25
    /// V tem primeru si bomo ogledali možnost eksplicitne implementacije metode vmesnika
    /// </summary>
    interface IPiece
    {
        void Move(ChessBoardField field);

        void Promote(ChessPiece toPiece);

        bool IsAlive { get; set; }

        ChessBoardField Position { get; }

        /// <summary>
        /// Z novo verzijo C# je dovoljeno predvideti tudi implementacije abstraktnih metod
        /// </summary>
        abstract void Move2(ChessBoardField field);
    }

    public abstract class ChessPiece : IPiece, ICareerObject
    {
        public ChessPiece(ChessBoardField start)
        {
            this.position = start;
        }

        public virtual double ChessWeight { get; protected set; }

        public override string ToString()
        {
            return $"Sem šahovska figura z vrednostjo {this.ChessWeight}.";
        }

        /// <summary>
        /// Implicitna implementacija metode
        /// </summary>
        public virtual void Move(ChessBoardField field)
        {
            this.position = field;
        }

        public abstract void Move2(ChessBoardField field);

        /// <summary>
        /// Eksplicitna implementacija metode iz IPiece
        /// </summary>
        void IPiece.Promote(ChessPiece toPiece)
        {
            Console.WriteLine($"(EKSPLICITNO iz IPiece) Jaz, figura kmet, se želim promovirati v figuro:\n {toPiece.GetType()}");
        }

        /// <summary>
        /// Implementacija metode z istim imenom v razredu
        /// </summary>
        public void Promote(ChessPiece toPiece)
        {
            Console.WriteLine($"(Metoda iz ChessPiece) Jaz, figura kmet, se želim promovirati v figuro:\n {toPiece.GetType()}");
        }

        void ICareerObject.Promote(ChessPiece toPiece)
        {
            Console.WriteLine($"(EKSPLICITNO iz ICareerObject) Jaz, figura kmet, se želim promovirati v figuro:\n {toPiece.GetType()}");
        }

        public bool IsAlive { get; set; }

        private ChessBoardField position;
        public ChessBoardField Position 
        {
            get
            {
                return position;
            } 
        }
    }

    /// <summary>
    /// Drugi vmesnik z metodo, ki nosi enako ime kot metoda v vmesniku IPiece
    /// </summary>
    interface ICareerObject
    {
        void Promote(ChessPiece toPiece);
    }

    public class Rook : ChessPiece
    {
        public Rook(ChessBoardField start) : base(start)
        {
            this.ChessWeight = 4.9;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nMoje ime je {this.GetType()}";
        }

        public override void Move(ChessBoardField field)
        {
            if (this.Position.X != field.X && this.Position.Y != field.Y)
                throw new Exception("Nedovoljen premik!");

            base.Move(field);
        }

        public override void Move2(ChessBoardField field)
        {
            if (this.Position.X != field.X && this.Position.Y != field.Y)
                throw new Exception("Nedovoljen premik!");

            base.Move(field);
        }
    }

    public class Player
    {
        /// <summary>
        /// Lastnost, ki vsebuje trenutne figure igralca
        /// </summary>
        public List<ChessPiece> MyPieces { get; } = new List<ChessPiece>();
    }

    /// <summary>
    /// Definiramo si struct za shranjevanje koordinat na šahovski plošči
    /// </summary>
    public struct ChessBoardField
    {
        /// <summary>
        /// Vodoravna koordinata
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Navpična koordinata
        /// </summary>
        public int Y { get; set; }

        public override string ToString()
        {
            return $"({this.X},{this.Y})";
        }
    }
}
