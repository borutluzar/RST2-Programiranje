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
                                            select new 
                                            { 
                                                animal.Species, 
                                                animal.HasTail 
                                            }; // Pripravimo anonimen objekt. Več v Arh, Q19.
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
                                            where animal.HasTail && animal.NumberOfLegs <= 4 // Filtriranje
                                            orderby animal.NumberOfLegs descending, animal.Species                                            
                                            select animal;                         
                        Console.WriteLine("\nSplošna filtrirana poizvedba");
                        queryFiltered.ReadEnumerable();
                    }
                    break;
            }
            // Več o sintaksi z uporabo "join" in "group by" v Arh, Q51 in Arh, Q54.
        }
    }
}
