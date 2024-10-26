using System;
using System.Collections.Generic;

namespace ObjektniKoncepti
{
    internal static class Indexers
    {
        /// <summary>
        /// Metoda za prikaz primerov uporabe indekserjev
        /// </summary>
        internal static void ExampleIndexers()
        {
            // Primer tabele:
            int[] tabelica = new int[3];
            tabelica[0] = 12;

            // Ustvarimo nov objekt, ki ima definiran indekser.
            MyIndexerClass myIndexerClass = new MyIndexerClass();

            // Nastavimo dve vrednosti
            myIndexerClass["Jabolko"] = 12.0;
            myIndexerClass["Hruška"] = 17.0;

            // Preberimo in izpišimo nastavljeni vrednosti
            Console.WriteLine($"Cena jabolka={myIndexerClass["Jabolko"]}, " +
                $"hruška={myIndexerClass["Hruška"]}");


            // Indekser lahko definiramo tudi s tabelo ali seznamom
            MyIndexerClassWithArray myIndexerWithArray = new MyIndexerClassWithArray(10);
            myIndexerWithArray[9] = 13.4;
            Console.WriteLine($"Cena jabolka={myIndexerWithArray[0]}, " +
                $"hruška={myIndexerWithArray[9]}");


            // Primer več-dimenzionalnega indekserja
            MyIndexerMultiDimTuple appleStore = new();
            appleStore["Jabolko", "Jonagold"] = 13.2;
            appleStore["Jabolko", "Topaz"] = 11.2;
            appleStore["Hruška", "Viljamovka"] = 17.2;

            Console.WriteLine($"Cena jabolko Jonagold={appleStore["Jabolko","Jonagold"]}");


            StudentGeneration sg = new StudentGeneration();
            sg.FirstEnrolmentYear = 2024;
            sg.ProgramName = "RST";

            // Dodajmo nekaj študentov
        }
    }

    /// <summary>
    /// Arh, Q34
    /// Indekserji ("Indexers") so podobni običajnim lastnostim,
    /// le da prejmejo dodaten parameter
    /// </summary>
    internal class MyIndexerClass
    {
        /// <summary>
        /// Definiramo polje, ki bo hranilo vrednosti
        /// </summary>
        private Dictionary<string, double> dicPrices = new Dictionary<string, double>();

        /// <summary>
        /// Indekserji vzamejo vrednosti neposredno iz slovarja
        /// </summary>
        /// <param name="article">Parameter, ki predstavlja indeks v slovarju</param>
        /// <returns>Vrednost na danem indeksu</returns>
        public double? this[string article]
        {
            get
            {
                return this.dicPrices.ContainsKey(article) ?
                    (double?)this.dicPrices[article] : null;
            }
            set
            {
                // Popravljanje vnosa
                if (this.dicPrices.ContainsKey(article))
                    this.dicPrices[article] = (double)value;
                // Dodajanje novega vnosa
                else
                    this.dicPrices.Add(article, (double)value);
            }
        }
    }


    /// <summary>
    /// Indekserje lahko definiramo tudi s tabelo ali seznamom,
    /// vendar moramo takrat za ključe uporabiti cela števila tipa int
    /// </summary>
    internal class MyIndexerClassWithArray
    {
        /// <summary>
        /// Definiramo polje, ki bo hranilo vrednosti
        /// </summary>
        private double[] dicPrices;

        public MyIndexerClassWithArray(int size)
        {
            this.dicPrices = new double[size];
        }

        /// <summary>
        /// Indekserji vzamejo vrednosti neposredno iz slovarja
        /// </summary>
        /// <param name="article">Parameter, ki predstavlja indeks v slovarju</param>
        /// <returns>Vrednost na danem indeksu</returns>
        public double? this[int index]
        {
            get
            {
                if (index < 0 || index > dicPrices.Length - 1)
                    return null;
                return this.dicPrices[index];
            }
            set
            {
                // Popravljanje vnosa
                if (index < dicPrices.Length && index >= 0)
                    this.dicPrices[index] = (double)value;
                // Dodajanje novega vnosa
                else
                    throw new Exception("The index is out of range!");
            }
        }
    }

    internal class MyIndexerMultiDim
    {
        /// <summary>
        /// Definiramo polje, ki bo hranilo vrednosti
        /// </summary>
        private double[,] dicPrices;

        public MyIndexerMultiDim(int sizeType, int sizeId)
        {
            this.dicPrices = new double[sizeType, sizeId];
        }

        /// <summary>
        /// Indekserji vzamejo vrednosti neposredno iz slovarja
        /// </summary>
        /// <param name="article">Parameter, ki predstavlja indeks v slovarju</param>
        /// <returns>Vrednost na danem indeksu</returns>
        public double? this[int type, int id]
        {
            get
            {
                return this.dicPrices[type, id];
            }
            set
            {
                this.dicPrices[type, id] = (double)value;
            }
        }
    }


    internal class MyIndexerMultiDimTuple
    {
        /// <summary>
        /// Definiramo polje, ki bo hranilo vrednosti
        /// </summary>
        private Dictionary<(string, string), double> dicPrices = new();

        /// <summary>
        /// Indekserji vzamejo vrednosti neposredno iz slovarja
        /// </summary>
        /// <param name="article">Parameter, ki predstavlja indeks v slovarju</param>
        /// <returns>Vrednost na danem indeksu</returns>
        public double? this[string type, string name]
        {
            get
            {
                return this.dicPrices[(type, name)];
            }
            set
            {
                this.dicPrices[(type, name)] = (double)value;
            }
        }
    }


    /// <summary>
    /// Še en primer razreda, ki uporablja indekserje. Imamo objekt, ki predstavlja generacijo študentov.
    /// Vsaka generacija ima dve lastnosti - leto prvega vpisa in študijski program,
    /// indeksirani pa so študentje generacije.
    /// </summary>
    public class StudentGeneration
    {
        public string ProgramName { get; set; }

        public int FirstEnrolmentYear { get; set; }

        private Dictionary<int, Student> dicStudents = new Dictionary<int, Student>();
        public Student this[int enrolmentNumber]
        {
            get
            {
                return dicStudents.ContainsKey(enrolmentNumber) ? dicStudents[enrolmentNumber] : null;
            }
            set
            {
                // Študenta dodamo samo, če še ne obstaja.
                if (dicStudents.ContainsKey(enrolmentNumber))
                    throw new Exception($"A student with enrolment number {enrolmentNumber} " +
                        $"already exists! Use Update method for changing his properties.");
                dicStudents[enrolmentNumber] = value;
            }
        }
    }

    public class Student
    {
        public string EnrolmentNumber { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public bool IsNull => throw new NotImplementedException();
    }
}
