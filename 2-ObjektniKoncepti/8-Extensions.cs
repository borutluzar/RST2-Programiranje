using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti.Extensions
{
    public static class Extensions
    {
        public static void TestExtensions()
        {
            string denis = "Denis";
            Console.WriteLine($"Denis vsebuje? {denis.ContainsVowels()}");

            ChessBoardField field = new ChessBoardField()
            {
                X = 4,
                Y = 3
            };

            // Klic razširitvene metode
            Console.WriteLine($"Klic razširitvene metode: {(field.IsWhite() ? "Bela" : "Črna")}");

            // Preverimo klic metode ToString() - vedno se kličejo metode iz razreda, če obstajajo
            Console.WriteLine($"Klic (razširitvene?) metode ToString {field.ToString()}");

            // Do razširitvene metode pridemo neposredno preko razreda
            Console.WriteLine($"Klic (razširitvene?) metode ToString čez statični razred {ExtensionsClass.ToString(field)}");
        }
    }

    /// <summary>
    /// Arh, Q33
    /// Razširitvene metode nam omogočijo razširiti funkcionalnosti objektov
    /// po naši meri, brez implementacije podrazreda
    /// </summary>
    public static class ExtensionsClass
    {
        public static bool ContainsVowels(this string str)
        {
            if (str.Contains('a') || str.Contains('e') || str.Contains('i') || str.Contains('o') || str.Contains('u'))
                return true;
            return false;
        }

        /// <summary>
        /// Dodamo razširitveno metode za naše (lokalne) potrebe
        /// </summary>
        public static bool IsWhite(this ChessBoardField field)
        {
            return (field.X + field.Y) % 2 == 0 ? false : true;
        }

        /// <summary>
        /// Metoda poskuša povoziti obstoječo metodo ToString.
        /// Prevajalnik nam to dovoli
        /// </summary>
        public static string ToString(this ChessBoardField field)
        {
            return $"[{field.X}:{field.Y}]";
        }

        // Dodajmo metodo za izpis elementov seznama.
    }

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
