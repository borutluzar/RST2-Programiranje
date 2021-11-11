﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti.Interfaces
{
    /// <summary>
    /// Metoda za testiranje vmesnikov
    /// </summary>
    public static class Interfaces
    {
        public static void TestInterfaces()
        {
            // Ustvarimo dve polji na plošči
            ChessBoardField fieldStart = new ChessBoardField() { X = 1, Y = 1 };
            ChessBoardField fieldEnd = new ChessBoardField() { X = 2, Y = 2 };

            ChessPiece piece = new ChessPiece(fieldStart);

            Console.WriteLine($"Trenutna pozicija figure piece je {piece.Position}");
            Console.WriteLine();

            // Premaknimo figuro
            piece.Move(fieldEnd);
            Console.WriteLine("Premaknemo...");
            Console.WriteLine($"Trenutna pozicija figure piece je {piece.Position}");


            // Premaknimo trdnjavo
            Console.WriteLine("\n");
            Rook rook = new Rook(fieldStart);
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
            position = field;
            //this.Position = field;
        }

        public bool IsAlive { get; set; }

        private ChessBoardField position;
        public ChessBoardField Position 
        {
            get
            {
                return position;
            } 
            /*private set 
            {
                position = value;
            } */
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
                throw new Exception("Nedovoljen premik!");

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
}
