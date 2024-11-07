using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti.Interfaces
{
    /// <summary>
    /// Razred s primeri vmesnikov
    /// 
    /// Dodatno branje: 
    /// Vmesniki od C# 9.0 dalje omogočajo definiranje statičnih abstraktnih metod, ki jih 
    /// pri predmetu ne bomo obravnavali, primere uporabe pa lahko najdete tukaj:
    /// https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/static-virtual-interface-members
    /// </summary>
    public static class Interfaces
    {
        public static void InterfacesExample()
        {
            // Ustvarimo dve polji na plošči
            ChessBoardField fieldStart = new ChessBoardField() { X = 1, Y = 1 };
            ChessBoardField fieldEnd = new ChessBoardField() { X = 2, Y = 2 };

            // Razred ChessPiece implementira vmesnik IPiece,
            // ki določa, katere metode mora razred implementirati
            ChessPiece piece = new ChessPiece(fieldStart);

            Console.WriteLine($"Trenutna pozicija figure piece je {piece.Position}");
            Console.WriteLine();

            // Premaknimo figuro
            piece.Move(fieldEnd);
            Console.WriteLine("Premaknemo...");
            Console.WriteLine($"Trenutna pozicija figure piece je {piece.Position}");


            // Premaknimo trdnjavo
            Console.WriteLine("\n");
            //Rook rook = new Rook(fieldStart);
            IPiece rook = new Rook(fieldStart);
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

            Console.WriteLine();
        }


        /// <summary>
        /// Še en primer uporabe vmesnika. 
        /// Pripravimo vmesnik za standardne metapodatke ter razmislimo, katere razrede bi lahko razširili z njim
        /// </summary>
        public static void ExampleMetaData()
        {
            // Ustvarimo štiri instance različnih tipov,
            // ki imajo skupno lastnost, da nosijo neke tipe metapodatkov
            Application app = new Application() { Organization = "FIŠ", Author = "Borut", DateCreated = DateTime.Now };
            File file = new File();
            MastersThesis ms = new MastersThesis();
            Exhibition ex = new Exhibition();

            // Dodajmo jih v seznam, ki kot vrednosti dobi tip IMetaData
            List<IMetaData> lstObjects = new List<IMetaData>()
            { 
                app,
                file,
                ms,
                ex
            };

            // Ker imajo vsi objekti seznama na voljo enako metodo,
            // lahko zanje izvajamo program na enak način
            // (npr. izpišemo metapodatke),
            // pa čeprav gre za nesorodne instance s stališča dedovanja
            foreach (IMetaData obj in lstObjects)
            {
                Console.WriteLine($"\nObject metadata: " +
                    $"- type: {obj.GetType()}" +
                    $"- author: {obj.Author}" +
                    $"- date created: {obj.DateCreated}" +
                    $"- organization: {obj.Organization} \n");

                if (obj is IDocumentMetaData)
                {
                    Console.WriteLine($"\nObject metadata: " +
                        $"- title: {((IDocumentMetaData)obj).Title}");
                }
            }
        }
    }


    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/interfaces/
    /// - Vmesniki določajo definicijo skupine funkcionalnosti,
    ///   ki jih mora razred implementirati.
    /// - Tovrstna "zaveza" omogoči programiranje splošnega obnašanja objektov, 
    ///   ki implementirajo dani vmesnik - npr. vse šahovske figure imajo metodo premakni.
    ///   Premik ni lasten samo šahovskim figuram, ampak tudi drugim.
    /// - Posamezen razred lahko implementira več vmesnikov, medtem ko deduje samo od enega razreda
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

        double ChessWeight { get; }

        ChessBoardField Position { get; set; }
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

    public class GoPiece : IPiece
    {
        public bool IsAlive 
        { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        public double ChessWeight => throw new NotImplementedException();

        public ChessBoardField Position 
        { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        public void Move(ChessBoardField field)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// V tem primeru naj razred implementira vmesnik IPiece,
    /// zato moramo implementirati tudi vse zahtevane metode in lastnosti vmesnika
    /// </summary>
    internal class ChessPiece : IPiece
    {
        public ChessPiece(ChessBoardField start)
        {
            this.position = start;
        }

        public double ChessWeight { get; protected set; }

        public override string ToString()
        {
            return $"Sem šahovska figura z vrednostjo {this.ChessWeight}.";
        }

        /// <summary>
        /// Implementiramo metodo za premik figure.
        /// Vsaka figura ima svoja pravila za premike, 
        /// zato bomo to metodo zelo verjetno v vsakem izmed podrazredov prepisali ("override").
        /// V ta namen jo označimo za virtualno.
        /// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual?f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(virtual_CSharpKeyword);k(DevLang-csharp)%26rd%3Dtrue
        /// </summary>
        /// <param name="field">Polje, kamor naj se figura premakne</param>
        public virtual void Move(ChessBoardField field)
        {
            //position = field;
            this.Position = field;
        }

        public bool IsAlive { get; set; }

        private ChessBoardField position;

        public ChessBoardField Position
        {
            get
            {
                return position;
            }
            set 
            {
                position = value;
            }
        }
    }

    /// <summary>
    /// V konkretnem podrazredu metodo Move zapišimo tako, 
    /// da preveri, če je premik metode možen.
    /// </summary>
    internal class Rook : ChessPiece
    {
        public Rook(ChessBoardField start) : base(start)
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
        /// <param name="field">Polje na plošči</param>
        public override void Move(ChessBoardField field)
        {
            // Pravilo za premik trdnjave
            if (this.Position.X != field.X && this.Position.Y != field.Y)
            {
                throw new Exception("Nedovoljen premik!");
            }

            base.Move(field);
        }
    }

    internal class Queen : ChessPiece
    {
        public Queen(ChessBoardField start) : base(start)
        {
            this.ChessWeight = 4.9;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nMoje ime je {this.GetType()}";
        }

        /// <summary>
        /// Kraljica se premika samo po diagonalah, linijah in vrstah
        /// </summary>
        /// <param name="field">Polje na plošči</param>
        public override void Move(ChessBoardField field)
        {
            // Pravilo za premik kraljice
            if (this.Position.X != field.X && this.Position.Y != field.Y)
                throw new Exception("Nedovoljen premik!");
            else if (Math.Abs(this.Position.X - field.X) != Math.Abs(this.Position.Y - field.Y))
            {
                throw new Exception("Nedovoljen premik!");
            }

            base.Move(field);
        }
    }

    internal class Player
    {
        /// <summary>
        /// Lastnost, ki vsebuje trenutne figure igralca
        /// </summary>
        public List<ChessPiece> MyPieces { get; } = new List<ChessPiece>();
    }


    /// <summary>
    /// Vmesnik z lastnostmi in metodami za razrede, 
    /// ki definirajo objekte, ki potrebujejo osnovne metapodatke
    /// </summary>
    interface IMetaData
    {
        string Author { get; set; }

        DateTime DateCreated { get; set; }

        string Organization { get; set; }
    }

    /// <summary>
    /// Razširitev vmesnika z dodatnima metodama,
    /// specifičnima za dokumente
    /// </summary>
    interface IDocumentMetaData : IMetaData
    {
        string Title { get; set; }

        DateTime DateModified { get; set; }
    }


    /// <summary>
    /// Razred, ki definira neko programsko rešitev
    /// </summary>
    internal class Application : IMetaData
    {
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public string Organization { get; set; }
    }

    /// <summary>
    /// Razred, ki definira datoteko
    /// </summary>
    internal class File : IMetaData
    {
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public string Organization { get; set; }
    }

    interface IEvaluatable
    {
        List<string> Committee { get; set; }

        void Present();
    }

    /// <summary>
    /// Razred, ki definira magistrsko nalogo
    /// </summary>
    internal class MastersThesis : IDocumentMetaData, IEvaluatable
    {
        public string Title { get; set; }
        public DateTime DateModified { get; set; }
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public string Organization { get; set; }
        public List<string> Committee { get; set ; }

        public void Present()
        {
            Console.WriteLine("I will present my master's thesis now!");
        }
    }

    /// <summary>
    /// Razred, ki definira razstavo
    /// </summary>
    internal class Exhibition : IMetaData
    {
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public string Organization { get; set; }
    }




}
