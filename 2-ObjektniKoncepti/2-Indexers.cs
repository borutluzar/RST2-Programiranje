using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti
{
    internal static class Indexers
    {
        /// <summary>
        /// Metoda za prikaz primerov uporabe indekserjev
        /// </summary>
        internal static void TestIndexers()
        {
            // Ustvarimo nov objekt, ki vsebuje indekser.
            IndexerExample ie = new IndexerExample();
            ie["Jabolko"] = 12.0;
            ie["Hruška"] = 17.0;

            Console.WriteLine($"Cena jabolka={ie["Jabolko"]}, hruška={ie["Hruška"]}");
        }
    }

    /// <summary>
    /// Arh, Q34
    /// Indekserji ("Indexers") so podobni običajnim lastnostim,
    /// le da prejmejo dodaten parameter
    /// </summary>
    internal class IndexerExample
    {
        /// <summary>
        /// Definiramo polje, ki bo hranilo vrednosti
        /// </summary>
        private Dictionary<string, double> dicPrice = new Dictionary<string, double>();

        /// <summary>
        /// Indekserji vzamejo vrednosti neposredno iz slovarja
        /// </summary>
        /// <param name="article">Parameter, ki predstavlja indeks v slovarju</param>
        /// <returns>Vrednost na danem indeksu</returns>
        public double? this[string article]
        {
            get
            {
                return this.dicPrice.ContainsKey(article) ? (double?)this.dicPrice[article] : null;
            }
            set
            {
                // Popravljanje vnosa
                if (this.dicPrice.ContainsKey(article))
                    this.dicPrice[article] = (double)value;
                // Dodajanje novega vnosa
                else
                    this.dicPrice.Add(article, (double)value);
            }
        }
    }
}
