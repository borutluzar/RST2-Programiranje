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
    static class Methods
    {
        enum MethodsSubsection
        {
            Basic = 1,
            Select = 2,
            SelectWithFunction = 3,
            Sort = 4,
            Filter = 5,
            CallOrder = 6,
            SkipAndTake = 7,
            Any = 8,
            All = 9,
            SelectMany = 10,
            Distinct = 11,
            Aggregate = 12
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
                        var queryGeneral = LINQDataSet.animals
                                                .Select(animal => animal);

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
                            .Select(animal => new 
                            { 
                                animal.Species, 
                                NumOfLegs = animal.NumberOfLegs,
                                Tail = animal.HasTail 
                            });
                        Console.WriteLine("\nSplošna poizvedba z izbranimi lastnostmi");

                        // Izpis anonimnega objekta zraven pripiše tudi imena lastnosti!
                        queryGeneral2.ReadEnumerable();
                    }
                    break;
                case MethodsSubsection.SelectWithFunction:
                    {
                        // V klicu Select funkcije lahko podamo kar funkcijo 
                        // (samo njeno ime!), vendar mora kot parameter prejeti objekt 
                        // takšnega tipa, kot je v prejšnjem seznamu.
                        var queryGeneral2 = LINQDataSet.animals
                            .Select(PodKind);

                        // Še primer, ko prilagodimo objekte seznama funkciji,
                        // da so parametri pravega tipa
                        var queryGeneral3 = LINQDataSet.animals
                                                .Select(animal => animal.NumberOfLegs)
                                                .Select(PodKind2);
                        
                        Console.WriteLine("\nSplošna poizvedba z izbranimi lastnostmi");
                        // Izpis anonimnega objekta zraven pripiše tudi imena lastnosti!
                        queryGeneral2.ReadEnumerable();

                        Console.WriteLine("\nPoizvedba s prilagoditvijo objektov funkciji");
                        queryGeneral3.ReadEnumerable();
                    }
                    break;
                case MethodsSubsection.Sort:
                    {
                        /*var queryOrdered = from animal in LINQDataSet.animals
                                           orderby animal.NumberOfLegs, animal.Species
                                           select animal;*/
                        var queryOrdered = LINQDataSet.animals
                                                .OrderByDescending(animal => animal.NumberOfLegs)
                                                .ThenBy(animal => animal.Species) // Za ureditev znotraj ureditve uporabimo ThenBy ali ThenByDescending
                                                .Select(animal => new { animal.Species, animal.HasTail });

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
                                                //.OrderBy(x => x.Species)  // se ne prevede, ker smo izbrali le Species
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
                case MethodsSubsection.Any:
                    {
                        // Metoda Any preveri, če v zbirki obstaja vsaj en element za dani pogoj
                        bool existsWithTail = LINQDataSet.animals.Any(x => x.HasTail);
                        Console.WriteLine($"\n{(existsWithTail ? "Obstaja vsaj ena žival z repom!" : "Ne obstaja žival z repom!")}");
                    }
                    break;
                case MethodsSubsection.All:
                    {
                        // Metoda All preveri, če vsi elementi v zbirki ustrezajo danemu pogoju
                        bool allWithTail = LINQDataSet.animals.All(x => x.HasTail);
                        Console.WriteLine($"\n{(allWithTail ? "Vse živali imajo rep!" : "Vse živali nimajo repa!")}");
                    }
                    break;
                case MethodsSubsection.SelectMany:
                    {
                        // Metodo SelectMany uporabimo, ko imajo objekti osnovnega predikata za lastnost sezname, 
                        // mi pa želimo narediti poizvedbo po teh seznamih (s tem se izognemo dvojnim zankam).
                        // Pri tem nas ne zanima, iz seznama katerega objekta smo zapise dobili.
                        var selectingMany = LINQDataSet.continents
                                                .SelectMany(continent => continent.Animals);

                        // Pozor! Rezultat zbere vse elemente, ki jih najde, tudi če so enaki.

                        // Poiščimo živali, ki imajo štiri noge
                        var querySelectMany = selectingMany
                                                .Where(animalID => LINQDataSet.animals.Find(x => x.ID == animalID)?.NumberOfLegs == 4)
                                                .Select(animalID => LINQDataSet.animals.Find(x => x.ID == animalID).Species);
                        Console.WriteLine($"\nŽivali na vseh celinah, ki imajo štiri noge, so: ");
                        querySelectMany.ReadEnumerable();
                    }
                    break;
                case MethodsSubsection.Distinct:
                    {
                        // Paziti moramo na to, da se izbor ne naredi po različnih vnosih (dobili smo več enakih ID-jev)
                        // Da poskrbimo za to, uporabimo metodo Distinct. Tej metodi lahko kot parameter podamo tudi 
                        // objekt tipa IEqualityComparer, ki določi, kateri pari instanc naj bodo obravnavani kot enaki (več v Arh, Q52).
                        var selectingManyDistinct = LINQDataSet.continents
                                                .SelectMany(continent => continent.Animals)
                                                .Distinct()
                                                .Where(animalID => LINQDataSet.animals.Find(x => x.ID == animalID)?.NumberOfLegs == 4)
                                                .Select(animalID => LINQDataSet.animals.Find(x => x.ID == animalID).Species)
                                                .OrderBy(x => x);
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
                        //var queryCount = LINQDataSet.animals.Max(x => x.ID);
                        //var queryCountBy = LINQDataSet.animals.MaxBy(x => x.ID);

                        // Bolj zanimiva je metoda Aggregate. 
                        // To je metoda, ki izvede ukaz na vseh elementih zaporedja,
                        // pri čemer upošteva informacije iz predhodnih ukazov na elementih zaporedja
                        
                        // Poiščimo vsoto nog vseh živali:
                        var queryAggNumLegs = LINQDataSet.animals
                                        .Aggregate<Animal, int, int>( // Tip elementa seznama, tip iskane vrednosti, tip rezultata
                                                seed: default(int), // Določitev začetne vrednosti iskane instance
                                                            // Prehod po vseh instancah z upoštevanjem pogoja
                                                (numLegs, animal) => numLegs + animal.NumberOfLegs,
                                                numLegs => numLegs // Rezultat
                                            );
                        Console.WriteLine($"\nŽivali imajo skupno {queryAggNumLegs} nog!");

                        // Poiščimo žival, ki ima najmanj nog (razmislite, s katero drugo funkcijo
                        // bi lahko dobili enak rezultat)
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


                        // Primer
                        // Iščemo žival z repom, ki ima največ nog
                        var queryAggMaxLegsWithTailAnimal = LINQDataSet.animals
                            .OrderBy(animal => animal.Species)
                            .Aggregate<Animal, Animal, string>( // Tip elementa seznama, tip iskane vrednosti, tip rezultata
                                    seed: null, // Določitev začetne vrednosti iskane instance
                                                // Prehod po vseh instancah z upoštevanjem pogoja
                                    (maxLegsWithTail, animal) => ((maxLegsWithTail == null && animal.HasTail)
                                                                    ||
                                                        (maxLegsWithTail != null && maxLegsWithTail.NumberOfLegs < animal.NumberOfLegs && animal.HasTail)) 
                                                                ? animal : maxLegsWithTail,
                                    maxLegsWithTail => maxLegsWithTail?.Species // Rezultat, ki ni nujno instanca, ampak le kakšna izmed lastnosti
                                        );
                        Console.WriteLine($"\nNajmanj nog ima {queryAggMaxLegsWithTailAnimal}!");
                    }
                    break;
            }
        }

        private static Pod PodKind(Animal animal)
        {
            switch (animal.NumberOfLegs)
            {
                case 1: return Pod.Monopod;
                case 2: return Pod.Bipod;
                case 4: return Pod.Tetrapod;
                case 6: return Pod.Hexapod;
                case 8: return Pod.Octopod;
                default: return Pod.Uknownopod;
            }
        }

        private static Pod PodKind2(int numberOfLegs)
        {
            switch (numberOfLegs)
            {
                case 1: return Pod.Monopod;
                case 2: return Pod.Bipod;
                case 4: return Pod.Tetrapod;
                case 6: return Pod.Hexapod;
                case 8: return Pod.Octopod;
                default: return Pod.Uknownopod;
            }
        }

        enum Pod
        {
            Monopod,
            Bipod,
            Tetrapod,
            Hexapod,
            Octopod,
            Uknownopod
        }
    }
}
