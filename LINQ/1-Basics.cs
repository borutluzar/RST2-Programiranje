using CommonFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace LINQ
{
    /// <summary>
    /// Arh, Q49
    /// LINQ - Language INtegrated Query
    /// Je posebna oblika pisanja poizvedb v podatkovnih strukturah.
    /// Uporabljamo ga lahko za poizvedbe po .Net zbirkah, relacijskih podatkovnih bazah ali pa po XML dokumentih.
    /// Z njim se izognemo prirejanju poizvedb za vsak posamezen tip podatkovnih struktur.
    /// 
    /// Ne pozabite vključiti paketa System.Linq!
    /// </summary>
    static class Basics
    {
        enum BasicsSubsection
        {
            Basic = 1,
            Select = 2,
            Sort = 3,
            Filter = 4
        }

        public static void BasicLINQSyntax()
        {
            switch (InterfaceFunctions.ChooseSection<BasicsSubsection>())
            {
                case BasicsSubsection.Basic:
                    {
                        // LINQ sintaksa je zelo podobna sintaksi SQL,
                        // vendar ima malo spremenjen vrstni red
                        // Select je vedno na koncu (lahko izberemo posamezne lastnosti)
                        var queryGeneral = from animal in LINQDataSet.animals
                                           select animal;
                        Console.WriteLine("\nSplošna poizvedba o vseh elementih");
                        queryGeneral.ReadEnumerable();
                    }
                    break;
                case BasicsSubsection.Select:
                    {
                        // Izberemo lahko le nekatere lastnosti
                        // in jih postavimo v nov (anonimen) objekt
                        var queryGeneral2 = from animal in LINQDataSet.animals
                                            select new { animal.Species, animal.HasTail }; // Pripravimo anonimen objekt. Več v Arh, Q19.
                        Console.WriteLine("\nSplošna poizvedba z izbranimi lastnostmi");
                        // Izpis anonimnega objekta zraven pripiše tudi imena lastnosti!
                        queryGeneral2.ReadEnumerable();
                    }
                    break;
                case BasicsSubsection.Sort:
                    {
                        // Elemente lahko uredimo - uporabimo spremenljivko animal
                        var queryOrdered = from animal in LINQDataSet.animals
                                           orderby animal.NumberOfLegs descending
                                           select animal;
                        Console.WriteLine("\nSplošna urejena poizvedba");
                        queryOrdered.ReadEnumerable();
                    }
                    break;
                case BasicsSubsection.Filter:
                    {
                        // Ali filtriramo - spet uporabimo spremenljivko animal
                        var queryFiltered = from animal in LINQDataSet.animals
                                            orderby animal.Species
                                            where animal.HasTail && animal.NumberOfLegs <= 4 // Filtriranje
                                            select animal;
                        Console.WriteLine("\nSplošna filtrirana poizvedba");
                        queryFiltered.ReadEnumerable();
                    }
                    break;
            }
            // Več o sintaksi z uporabo "join" in "group by" v Arh, Q51 in Arh, Q54.
        }

        enum MethodsSubsection
        {
            Basic = 1,
            Select = 2,
            Sort = 3,
            Filter = 4,
            CallOrder = 5,
            SkipAndTake = 6,
            Any = 7,
            All = 8,
            SelectMany = 9,
            Distinct = 10,
            Aggregate = 11
        }

        /// <summary>
        /// Arh, Q50
        /// Za vse ukaze, ki smo si jih ogledali v metodi BasicLINQSyntax,
        /// imamo na voljo tudi razširitvene metode, 
        /// ki nam omogočijo klice metod za posamezno akcijo
        /// </summary>
        public static void MethodLINQSyntax()
        {
            // LINQ sintaksa, ki smo jo predstavili zgoraj, 
            // se prevede v klice funkcij. Te lahko uporabimo tudi mi.

            switch (InterfaceFunctions.ChooseSection<MethodsSubsection>())
            {
                case MethodsSubsection.Basic:
                    {
                        /*var queryGeneral = from animal in LINQDataSet.animals
                                           select animal; */
                        // Zgornjo kodo tako prepišemo v
                        var queryGeneral = LINQDataSet.animals.Select(animal => animal);
                        // Metoda Select 'projicira' vsak element zbirke v podano obliko
                        Console.WriteLine("\nSplošna poizvedba o vseh elementih");
                        queryGeneral.ReadEnumerable();
                    }
                    break;
                case MethodsSubsection.Select:
                    {
                        /*var queryGeneral2 = from animal in LINQDataSet.animals
                                            select new { animal.Species, animal.HasTail };*/
                        var queryGeneral2 = LINQDataSet.animals
                            .Select(animal => new { animal.Species, animal.HasTail });
                        Console.WriteLine("\nSplošna poizvedba z izbranimi lastnostmi");
                        // Izpis anonimnega objekta zraven pripiše tudi imena lastnosti!
                        queryGeneral2.ReadEnumerable();
                    }
                    break;
                case MethodsSubsection.Sort:
                    {
                        /*var queryOrdered = from animal in LINQDataSet.animals
                                           orderby animal.Species
                                           select animal;*/
                        var queryOrdered = LINQDataSet.animals
                                                .OrderByDescending(animal => animal.NumberOfLegs)
                                                .ThenBy(animal => animal.Species)
                                                .Select(animal => animal);
                        Console.WriteLine("\nSplošna urejena poizvedba");
                        queryOrdered.ReadEnumerable();
                    }
                    break;
                case MethodsSubsection.Filter:
                    {
                        /*var queryFiltered = from animal in LINQDataSet.animals
                                            orderby animal.Species
                                            where animal.HasTail
                                            select animal;*/
                        var queryFiltered = LINQDataSet.animals
                                                .OrderBy(animal => animal.Species) // Uredimo. Imamo tudi OrderByDescending
                                                                                   // Za ureditev znotraj ureditve uporabimo ThenBy ali ThenByDescending
                                                .Where(animal => animal.HasTail) // Filtriramo
                                                .Select(animal => animal); // Povemo, katere lastnosti želimo v rezultatu
                        Console.WriteLine("\nSplošna filtrirana poizvedba");
                        queryFiltered.ReadEnumerable();
                    }
                    break;
                case MethodsSubsection.CallOrder:
                    {
                        // V standardni LINQ sintaksi je vrstni red ukazov pomemben
                        // oziroma moramo select zapisati na koncu.
                        // Pri razširitvenih metodah nam tega ni treba,
                        // vendar moramo vedeti, da se klici v teh primerih izvedejo
                        // v danem vrstnem redu.
                        var querySelectFirst = LINQDataSet.animals
                                                .Select(animal => animal.Species)
                                                //.OrderBy(animal => animal.Species)  // se ne prevede, ker smo izbrali le Species
                                                .OrderBy(species => species);
                        //.Where(animal => animal.HasTail); // se ne prevede, ker smo izbrali le Species
                        Console.WriteLine("\nPoizvedba samo z vrsto živali");
                        querySelectFirst.ReadEnumerable();
                    }
                    break;
                case MethodsSubsection.SkipAndTake:
                    {
                        // Poleg osnovnih metod, ki omogočajo klice
                        // funkcij iz standardne sintakse, pa imamo na voljo še nekaj
                        // metod, ki poenostavijo delo s poizvedbami.
                        var queryAdditional = LINQDataSet.animals
                                                .OrderBy(animal => animal.Species)
                                                .Where(animal => animal.HasTail)
                                                .Select(animal => animal)
                                                .Skip(1)  // Preskoči prvih k (v našem primeru 1) zapisov
                                                .Take(1);  // Vzemi k (v našem primeru 1)  zapisov

                        // Metodi Take in Skip sta lahko uporabni npr. pri implementaciji listanja po zapisih (paginaciji)
                    }
                    break;
                case MethodsSubsection.All:
                    {
                        // Metoda Any preveri, če v zbirki obstaja vsaj en element za dani pogoj
                        var existsWithTail = LINQDataSet.animals.Any(x => x.HasTail);
                        Console.WriteLine($"\n{(existsWithTail ? "Obstaja vsaj ena žival z repom!" : "Ne obstaja žival z repom!")}");
                    }
                    break;
                case MethodsSubsection.Any:
                    {
                        // Metoda All preveri, če vsi elementi v zbirki ustrezajo danemu pogoju
                        var allWithTail = LINQDataSet.animals.All(x => x.HasTail);
                        Console.WriteLine($"\n{(allWithTail ? "Vse živali imajo rep!" : "Nimajo vse živali repa!")}");
                    }
                    break;
                case MethodsSubsection.SelectMany:
                    {
                        // Metodo SelectMany uporabimo, ko imajo objekti osnovnega predikata za lastnost sezname, 
                        // mi pa želimo narediti poizvedbo po teh seznamih (s tem se izognemo dvojnim zankam).
                        // Pri tem nas ne zanima, iz seznama katerega objekta smo zapise dobili.
                        var selectingMany = LINQDataSet.continents
                                                .SelectMany(continent => continent.Animals)                                                
                                                .Where(animalID => LINQDataSet.animals.Find(x => x.ID == animalID).NumberOfLegs == 4)                                                
                                                .Select(animalID => LINQDataSet.animals.Find(x => x.ID == animalID).Species);
                        Console.WriteLine($"\nŽivali na vseh celinah, ki imajo štiri noge, so: ");
                        selectingMany.ReadEnumerable();
                    }
                    break;
                case MethodsSubsection.Distinct:
                    {
                        // Paziti moramo na to, da se izbor ne naredi po različnih vnosih (dobili smo več enakih ID-jev)
                        // Da poskrbimo za to, uporabimo metodo Distinct. Tej metodi lahko kot parameter podamo tudi 
                        // objekt tipa IEqualityComparer, ki določi, kateri pari instanc naj bodo obravnavani kot enaki (več v Arh, Q52).
                        var selectingManyDistinct = LINQDataSet.continents
                                                .SelectMany(continent => continent.Animals)
                                                .OrderBy(x => x)
                                                .Distinct();
                        Console.WriteLine($"\nRazlične živali na vseh celinah so: ");
                        selectingManyDistinct.ReadEnumerable();
                    }
                    break;
                case MethodsSubsection.Aggregate:
                    {
                        // Podobno kot metodi All in Any, lahko uporabimo metodi Count in CountLong 
                        // za vračanje števila instanc, ki ustrezajo pogoju podanemu kot argument obeh metod.
                        // Brez argumenta metodi vrneta število vseh instanc v naboru.

                        // Na voljo imamo tudi metode Min, Max, Average ter Sum, ki vračajo to, kar povejo njihova imena.
                        // Bolj zanimiva je metoda Aggregate. 
                        // Poiščimo žival, ki ima najmanj nog
                        var queryAggMinLegsAnimal = LINQDataSet.animals
                                        .Aggregate<Animal, Animal, string>( // Tip elementa seznama, tip iskane vrednosti, tip rezultata
                                                seed: null, // Določitev začetne vrednosti iskane instance
                                                            // Prehod po vseh instancah z upoštevanjem pogoja
                                                (minLegs, animal) => (minLegs == null || minLegs.NumberOfLegs > animal.NumberOfLegs) ? animal : minLegs,
                                                minLegs => minLegs?.Species // Rezultat, ki ni nujno instanca, ampak le kakšna izmed lastnosti
                                            );
                        Console.WriteLine($"\nNajmanj nog ima {queryAggMinLegsAnimal}!");

                        var queryAggMinLegsNumber = LINQDataSet.animals
                                        .Aggregate<Animal, int, int>( // Tip elementa seznama, tip iskane vrednosti, tip rezultata
                                                seed: int.MaxValue, // Določitev začetne vrednosti iskane instance
                                                // Prehod po vseh instancah z upoštevanjem pogoja
                                                (minLegs, animal) => (minLegs == int.MaxValue || minLegs > animal.NumberOfLegs) ? animal.NumberOfLegs : minLegs,
                                                minLegs => minLegs // Rezultat
                                            );
                        Console.WriteLine($"\nNajmanjše število nog je {queryAggMinLegsNumber}!");
                    }
                    break;
            }
        }
    }
}
