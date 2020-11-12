using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PodatkovneStrukture
{
    static class ICollectionTest
    {
        /// <summary>
        /// Oglejmo si vmesnik ICollection<T> in metode, ki jih zagotovi.
        /// </summary>
        public static void MethodsOfICollection(Section section)
        {
            // Definirajmo zbirko 
            ICollection<string> zbirka = new List<string>() { "Luka", "Jernej", "Dejan", "Denis", "Tilen", "Jernej", "Jakob", "Samo" };

            switch (section)
            {
                case Section.ICollection:
                    {
                        // Preizkusimo metode, ki jih ponuja ICollection
                        Console.WriteLine($"Count pred Add: {zbirka.Count}");
                        zbirka.Add("Borut");
                        Console.WriteLine($"Count pred Remove: {zbirka.Count}");
                        zbirka.Remove("Borut");
                        Console.WriteLine($"Count pred Clear: {zbirka.Count}");
                        zbirka.Clear();
                        Console.WriteLine($"Count na koncu: {zbirka.Count}");
                    }
                    break;
                case Section.IList:
                    {
                        // Spremenimo zbirko v instanco IList
                        IList<string> seznam = (IList<string>)zbirka;
                        
                        Console.WriteLine($"seznam[0]: {seznam[0]}");
                        seznam.Insert(0, "Borut");
                        Console.WriteLine($"seznam[0]: {seznam[0]}");
                        seznam.RemoveAt(1);
                        Console.WriteLine($"seznam[1]: {seznam[1]}");
                    }
                    break;
                case Section.ISet:
                    {
                        // In v Set - klic se prevede, 
                        // vendar ob izvajanju javi napako InvalidCastException, 
                        // ker List ne implementira vmesnika ISet<T>
                        //ISet<string> mnozica = (ISet<string>)zbirka;

                        // Definirajmo si novo množico
                        ISet<string> mnozica = new HashSet<string>() { "Luka", "Jernej", "Dejan", "Denis", "Tilen", "Jernej", "Jakob", "Samo" };

                        // Preštejmo elemente - eden manjka!
                        Console.WriteLine($"Število elementov v množici: {mnozica.Count}");
                        // Za dodajanje imamo na voljo Add, 
                        // ki vrne true ali false glede na uspeh dodajanja
                        bool isInserted = mnozica.Add("Borut");                        
                        Console.WriteLine($"Element " +
                            $"{(isInserted ? "je bil dodan." : "ni bil dodan")}");
                        Console.WriteLine($"Število elementov v množici: {mnozica.Count}");
                        
                        // Metodo add iz vmesnika ICollection lahko kličemo le eksplicitno
                        //((ICollection<string>)mnozica).Add("Borut");
                    }
                    break;
                case Section.IDictionary:
                    {
                        // Ustvarimo slovar z imeni dni
                        IDictionary<int, string> dnevi = new Dictionary<int, string>();

                        // Dodajmo nekaj vnosov
                        dnevi[1] = "ponedeljek";
                        dnevi[2] = "torek";
                        dnevi[5] = "petek";

                        Console.WriteLine($"Elementi v dnevi: {dnevi.Count}");

                        // Uporabimo lahko metodo Add
                        dnevi.Add(3, "sreda");
                        Console.WriteLine($"Elementi v dnevi: {dnevi.Count}");

                        // Vrednost dobimo s pomočjo indekserja,
                        // paziti pa moramo, da ključ v slovarju obstaja
                        Console.WriteLine($"3. dan v tednu: {dnevi[3]}");
                        // Zato to vedno preverimo
                        if(dnevi.ContainsKey(4))
                            Console.WriteLine($"4. dan v tednu: {dnevi[4]}");

                        // Elemente lahko odstranimo zgolj z uporabo ključa
                        bool isRemoved = dnevi.Remove(1);
                        Console.WriteLine(
                            $"Element {(isRemoved ? "je odstranjen" : "ni v slovarju")}");
                        Console.WriteLine($"Elementi v dnevi: {dnevi.Count}");

                        // Seznama vseh ključev in vrednosti sta na voljo preko spodnjih lastnosti
                        var kljuci = dnevi.Keys;
                        var vrednosti = dnevi.Values;
                        Console.WriteLine($"Število ključev: {kljuci.Count}");
                        Console.WriteLine($"Število vrednosti: {vrednosti.Count}");
                    }
                    break;
            }

        }
    }

    /// <summary>
    /// Implementirajmo svojo strukturo.
    /// </summary>
    public class MySet<T> : ISet<T>
    {
        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public bool Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

}
