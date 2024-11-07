using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti.Extensions
{
    /// <summary>
    /// Razširitvene metode uporabimo, ko želimo razširiti funkcionalnosti instanc razredov,
    /// do katerih nimamo dostopa. Npr. če želimo stringu dodati metodo -- int NumberOfLetters(char letter) --
    /// </summary>
    public static class Extensions
    {
        public static void ExampleExtensions()
        {
            string filip = "téma";
            
            Console.WriteLine($"Filip vsebuje? {filip.ContainsVowels()}");
            Console.WriteLine($"Filip vsebuje? {ExtensionsClass.ContainsVowels(filip)}");
            

            char c = 'e';
            int numChars = filip.CountChars(c);
            Console.WriteLine($"Filip vsebuje {numChars} znakov '{c}'!");

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

            // Obe funkciji lahko kličemo v statičnem smislu, ne pa kot razširitveni na instanci
            // Napaka je šele ob klicu na instanci (ambiguity), ker ne vemo, katero od obeh uporabiti.
            // Pred tem ne, ker lahko imamo razširitvi definirani v različnih projektih, ki jih referenciramo v našem
            // in seveda nočemo napake že ob vključitvi dodatne kode, temveč šele ko izvedemo dvoumen klic!
            Console.WriteLine($"Klic (razširitvene?) metode ToString čez statični razred {ExtensionsClass.ToString2(field)}");
            Console.WriteLine($"Klic (razširitvene?) metode ToString čez statični razred {ExtensionsClass2.ToString2(field)}");
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
        /// Counts all characters equal to chr.
        /// </summary>
        public static int CountChars(this string str, char chr)
        {
            int counter = 0;
            foreach(char c in str.ToLower())
            {
                if(c == Char.ToLower(chr))
                {
                    counter++;
                }
            }
            return counter;
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

        public static string ToString2(this ChessBoardField field)
        {
            return $"[{field.X}:{field.Y}]";
        }

        // Dodajmo metodo za izpis elementov seznama.
    }

    public static class ExtensionsClass2
    {
        public static string ToString2(this ChessBoardField field)
        {
            return $"[{field.X}:{field.Y}]";
        }
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

        /*public bool IsWhite()
        {
            return (this.X + this.Y) % 2 == 0 ? false : true;
        }*/
    }

}
