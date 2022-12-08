using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti.Abstraction
{
    public static class Abstraction
    {
        public static void TestAbstraction()
        {
            ChessBoardField fieldStart = new ChessBoardField() { X = 1, Y = 1 };
            ChessBoardField fieldEnd = new ChessBoardField() { X = 1, Y = 2 };

            // Spodnja koda se ne prevede
            //ChessPiece piece = new ChessPiece(fieldStart);

            ChessPiece rook = new Rook(fieldStart);

            Console.WriteLine($"Trenutna pozicija figure rook je {rook.Position}");
            Console.WriteLine();

            // Premaknimo figuro
            try
            {
                rook.Move(fieldEnd);
                Console.WriteLine("Premaknemo...");
                Console.WriteLine($"Trenutna pozicija figure rook je {rook.Position}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Prišlo je do napake pri premiku figure {nameof(rook)}");
                Console.WriteLine(ex.Message);
            }
        }
    }


    /// <summary>
    /// V prejšnjem primeru smo se ukvarjali z vmesniki.
    /// Podoben koncept predstavljajo abstraktni razredi
    /// </summary>
    interface IPiece
    {
        /// <summary>
        /// Vsaka figura se zna premakniti na neko polje
        /// </summary>
        /// <param name="field">Polje, kamor se naj figura premakne</param>
        void Move(ChessBoardField field);

        /// <summary>
        /// Vmesniki definirajo tudi lastnosti v enaki obliki, 
        /// kot v razredu definiramo samodejno implementirane lastnosti.
        /// Vendar moramo lastnosti vmesnika dejansko implementirati v razredu.
        /// </summary>
        bool IsAlive { get; set; }

        ChessBoardField Position { get; }
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

    /// <summary>
    /// Arh, Q24
    /// "Šahovska figura" sama po sebi ne nosi nobene informacije, 
    /// dokler ne vemo, za katero figuro gre.
    /// Dejansko instance tega razreda nikoli ne bomo potrebovali,
    /// vedno samo instance podrazredov.
    /// V takih primerih, ko ne želimo dovoliti kreiranja instanc nadrazreda,
    /// označimo razred za abstrakten.
    /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract?f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(abstract_CSharpKeyword);k(DevLang-csharp)%26rd%3Dtrue
    /// </summary>
    public abstract class ChessPiece : IPiece
    {
        /// <summary>
        /// Konstruktorje abstraktnemu razredu lahko določimo
        /// </summary>
        public ChessPiece(ChessBoardField start)
        {
            this.position = start;
        }

        /// <summary>
        /// Konstruktor, ki nam omogoča določitev konstante VAL_COEFFICIENT tudi v 
        /// podrazredih.
        /// </summary>
        public ChessPiece(ChessBoardField start, double valCoeff)
        {
            this.position = start;
            VAL_COEFFICIENT = valCoeff;
        }

        public virtual double ChessWeight { get; protected set; }

        public override string ToString()
        {
            return $"Sem šahovska figura z vrednostjo {this.ChessWeight}.";
        }

        /// <summary>
        /// Definirajmo posebno konstanto, ki bo vidna samo v abstraktnem razredu,
        /// ker nočemo, da se ji vrednost spremeni v kakšni drugi situaciji.
        /// </summary>
        private readonly double VAL_COEFFICIENT = 1;

        /// <summary>
        /// Te metode nam ni treba implementirati v abstraktnem razredu, 
        /// saj jo bo vsaka metoda iz podrazreda morala povoziti
        /// </summary>
        /// <param name="field">Polje, kamor naj se figura premakne</param>
        public abstract void Move(ChessBoardField field);

        public bool IsAlive { get; set; }

        /// <summary>
        /// Implementirana lastnost mora biti zapisana eksplicitno
        /// </summary>
        private ChessBoardField position;
        public ChessBoardField Position
        {
            get
            {
                return position;
            }
            protected set 
            {
                position = value;
            }
        }
    }

    /// <summary>
    /// V konkretnem podrazredu metodo Move zapišimo tako, 
    /// da preveri, če je premik metode možen.
    /// </summary>
    public class Rook : ChessPiece
    {
        public Rook(ChessBoardField start) : base(start)
        {
            this.ChessWeight = 4.9;
        }

        /// <summary>
        /// V tem konstruktorju lahko nastavimo tudi vrednost spremeljivki,
        /// ki je vidna samo nadrazredu.
        /// </summary>
        public Rook(ChessBoardField start, double valCoeff) : base(start, valCoeff)
        {
            this.ChessWeight = 4.9;            
        }

        public override string ToString()
        {
            return base.ToString() + $"\nMoje ime je {this.GetType()}";
        }

        /// <summary>
        /// Trdnjava se premika samo po linijah in vrstah
        /// </summary>
        public override void Move(ChessBoardField field)
        {
            if (this.Position.X != field.X && this.Position.Y != field.Y)
                throw new Exception("Nedovoljen premik!");

            // Če je metoda v nadrazredu označena za abstraktno, 
            // moramo njeno implementacijo dokončati v podrazredu.
            // Pri tem moramo vedno dobro razmisliti, 
            // če lahko že v nadrazredu v metodi izvedemo nek košček kode, 
            // ki se naj izvede v vseh podrazredih. 
            // V tem primeru bo metoda v nadrazredu virtualna.
            //base.Move(field);
            this.Position = field; // Paziti moramo, da ima lastnost ustrezno vidnost...
            //Console.WriteLine("Premik je dovoljen, ampak nimam dostopa do this.Position! :(");
        }
    }

    public class Player
    {
        /// <summary>
        /// Lastnost, ki vsebuje trenutne figure igralca
        /// </summary>
        public List<ChessPiece> MyPieces { get; } = new List<ChessPiece>();
    }
}
