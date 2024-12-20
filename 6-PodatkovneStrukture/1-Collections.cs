﻿using CommonFunctions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PodatkovneStrukture
{
    static class ICollectionTest
    {
        /// <summary>
        /// Oglejmo si vmesnik ICollection<T> in metode, ki jih zagotovi.
        /// Arh, Q46
        /// Pregledamo še ostale glavne pod-vmesnike
        /// </summary>
        public static void MethodsOfICollection(StructuresSections section)
        {
            // Definirajmo zbirko 
            ICollection<string> zbirka = new List<string>() { "Blaž", "Luka", "Jernej", "Dejan", "Denis", "Tilen", "Jernej", "Jakob", "Samo" };
            Console.WriteLine($"{zbirka.WriteCollection()}\n");

            switch (section)
            {
                case StructuresSections.ICollection:
                    {
                        // Preizkusimo metode, ki jih ponuja ICollection
                        Console.WriteLine($"Count pred Add: {zbirka.Count}");
                        
                        zbirka.Add("Borut");
                        Console.WriteLine($"{zbirka.WriteCollection()}\n");
                        Console.WriteLine($"Count pred Remove: {zbirka.Count}");
                        
                        zbirka.Remove("Borut");
                        Console.WriteLine($"{zbirka.WriteCollection()}\n");
                        Console.WriteLine($"Count pred Clear: {zbirka.Count}");
                        
                        zbirka.Clear();
                        Console.WriteLine($"{zbirka.WriteCollection()}\n");
                        Console.WriteLine($"Count na koncu: {zbirka.Count}");
                    }
                    break;
                case StructuresSections.IList:
                    {
                        // Spremenimo zbirko v instanco IList
                        // Seznami so v resnici tabele, le da si rezervirajo več prostora in
                        // skrijejo operacije, ki jih moramo v tabelah izvesti sami (npr. dodajanje novega elementa na konec)
                        IList<string> seznam = (IList<string>)zbirka;
                        
                        Console.WriteLine($"seznam[0]: {seznam[0]}");
                        seznam.Insert(0, "Borut");
                        Console.WriteLine($"seznam[0]: {seznam[0]}");
                        seznam.RemoveAt(1);
                        Console.WriteLine($"seznam[1]: {seznam[1]}");
                    }
                    break;
                case StructuresSections.ISet:
                    {
                        // In v Set - klic se prevede, 
                        // vendar ob izvajanju javi napako InvalidCastException, 
                        // ker List ne implementira vmesnika ISet<T>
                        ISet<string> mnozica1 = (ISet<string>)zbirka;

                        // Definirajmo si novo množico
                        ISet<string> mnozica2 = new HashSet<string>() { "Luka", "Jernej", "Dejan", "Denis", "Tilen", "Jernej", "Jakob", "Samo" };

                        // Preštejmo elemente - eden manjka!
                        Console.WriteLine($"Število elementov v množici: {mnozica2.Count}");
                        
                        // Za dodajanje imamo na voljo Add, 
                        // ki vrne true ali false glede na uspeh dodajanja
                        bool isInserted = mnozica2.Add("Borut");                           
                        Console.WriteLine($"Element " +
                            $"{(isInserted ? "je bil dodan." : "ni bil dodan")}");
                        Console.WriteLine($"Število elementov v množici: {mnozica2.Count}");

                        // Metodo add iz vmesnika ICollection lahko kličemo le eksplicitno
                        ((ICollection<string>)mnozica2).Add("Borut");
                        Console.WriteLine($"{mnozica2.WriteCollection()}\n");

                        /*
                        ((ICollection<string>)mnozica2).Add("Borut");
                        Console.WriteLine($"{mnozica2.WriteCollection()}\n");
                        */
                    }
                    break;
                case StructuresSections.IDictionary:
                    {
                        // Ustvarimo slovar z imeni dni
                        IDictionary<int, string> dnevi = new Dictionary<int, string>();

                        // Dodajmo nekaj vnosov
                        dnevi.Add(7, "nedelja");
                        dnevi[1] = "ponedeljek";
                        dnevi[2] = "torek";
                        dnevi[5] = "petek";

                        Console.WriteLine($"Elementi v dnevi: {dnevi.Count}");
                        Console.WriteLine($"{dnevi.WriteCollection()}\n");

                        // Uporabimo lahko metodo Add - podamo ključ in vrednost
                        dnevi.Add(3, "sreda");
                        Console.WriteLine($"{dnevi.WriteCollection()}\n");
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
                        var kljuci = dnevi.Keys.ToList();
                        Console.WriteLine($"Tretji indeks: {kljuci[2]}");
                        var vrednosti = dnevi.Values;
                        Console.WriteLine($"Število ključev: {kljuci.Count}");
                        Console.WriteLine($"Število vrednosti: {vrednosti.Count}");

                        var lstDays = dnevi.OrderBy(x => x.Key).ToList();
                        Console.WriteLine($"Urejeno: {lstDays.WriteCollection()}\n");
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Implementirajmo svojo strukturo.
    /// </summary>
    public class MyList<T> : IList<T>
    {
        private readonly IList<T> myList;

        public MyList()
        {
            myList = new List<T>();
        }

        T IList<T>.this[int index] 
        { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        int ICollection<T>.Count => myList.Count;

        bool ICollection<T>.IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            if (item is string)
            {
                string name = "Borut";
                if (name.Equals(item))
                {
                    return;
                }
            }
            this.myList.Add(item);
        }

        void ICollection<T>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            foreach(var x in ((IEnumerable)this))
            {
                if (x.Equals(item))
                    return true;
            }
            return false;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return myList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList<T>.IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
    }
}
