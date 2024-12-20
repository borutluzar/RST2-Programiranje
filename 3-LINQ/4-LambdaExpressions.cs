﻿using CommonFunctions;
using System;
using System.Linq;

namespace LINQ
{
    /// <summary>
    /// Arh, Q57
    /// </summary>
    class LambdaExpressions
    {
        enum LambdaSubsections
        {
            FunctionWithLambda = 1,
            FunctionWithLambdaWithParameter = 2,
            ThreeFunctionSyntaxes = 3
        }

        public static void LambdaExpressionExamples()
        {
            // V LINQ imamo dve vrsti razširitvenih metod
            // - metode za vmesnik IEnumerable<T>, ko po podatkih poizvedujemo lokalno
            // - metode za vmesnik IQueryable<T>, ko poizvedujemo po podatkih na zunanjem viru

            // Preden si ogledamo razlike med njima, definirajmo lambda-izraz.
            // Lambda izrazi so oblike
            // (vhodni parametri) => ukaz   ali
            // (vhodni parameter) => {zaporedje ukazov}
            // Gre za skrajšan zapis funkcij. 

            switch (InterfaceFunctions.ChooseSection<LambdaSubsections>())
            {
                case LambdaSubsections.FunctionWithLambda:
                    {
                        // Celo definiramo jih lahko in kličemo na različnih mestih.                         
                        // (x, y) => (x + y, x - y)
                        Func<int, int, (int, int)> sum = (x, y) => (x + y, x - y);
                        Func<int, int, int, bool> isPitagorean = (x, y, z) => x * x + y * y == z * z;
                        Func<double, int> roundUp = x => (int)Math.Round(x, 0, MidpointRounding.AwayFromZero);
                        Func<double, int> roundDown = x => (int)Math.Round(x, 0);
                        double x = Math.PI;
                        var y = sum(4, 3);
                        
                        Console.WriteLine($"Seštejmo in odštejmo 4 in 3: {y}");
                        Console.WriteLine($"{(isPitagorean(5,12,13) ? "5, 12 in 13 je pitagorejska trojica" : "5, 12, 13 ni pitagorejska trojica!")}");
                        Console.WriteLine($"Zaokrožimo število {x}: {roundUp(x)}");
                        Console.WriteLine($"Zaokrožimo število 4.5 navzgor: {roundUp(4.5)}");
                        Console.WriteLine($"Zaokrožimo število 4.5 navzdol: {roundDown(4.5)}");
                        Console.WriteLine($"Zaokrožimo število 4.5001 navzgor: {roundUp(4.5001)}");
                        Console.WriteLine($"Zaokrožimo število 4.5001 navzdol: {roundDown(4.5001)}");
                    }
                    break;
                case LambdaSubsections.FunctionWithLambdaWithParameter:
                    {
                        // V definiciji lambda izraza lahko uporabimo tudi spremenljivke,
                        // ki so v bloku definicije vidne
                        int a = 11;
                        Func<int, int, (int, int)> sumWithOffset = (x, y) => (x + y + a, x - y + a);
                        var y = sumWithOffset(4, 3);
                        Console.WriteLine($"Seštejmo in odštejmo 4 in 3 z odmikom: {y}");
                    }
                    break;
                case LambdaSubsections.ThreeFunctionSyntaxes:
                    {
                        // LINQ poizvedbo torej lahko izvedemo na tri različne načine
                        var queryLambda = LINQDataSet.animals
                            .Where(x => x.NumberOfLegs < 4);
                        Console.WriteLine($"\nObičajni lambda izraz:");
                        queryLambda.ReadEnumerable();

                        var queryDelegate = LINQDataSet.animals
                            .Where(delegate (Animal x) { return x.NumberOfLegs < 4; });
                        Console.WriteLine($"\nAnonimna metoda:");
                        queryDelegate.ReadEnumerable();

                        var queryFunction = LINQDataSet.animals.Where(LessThan4Legs);
                        Console.WriteLine($"\nPosebej definirana funkcija:");
                        queryFunction.ReadEnumerable();
                                                

                        // Razlika med zgornjimi tremi klici je zgolj v sintaksi,
                        // pri izvedbi pa se bodo obnašale enako, če smo na IEnumerable<T> instanci,
                        // saj je parameter teh metod oblike Func<,> (npr. Func<T,bool>).


                        // POZOR!
                        // Pri metodah vmesnika IQueryable moramo kot parameter podati objekte
                        // tipa Expression<Func<,>> (npr. Expression<Func<T,bool>>).
                        // Razlika v kodi ne bo opazna, vsi trije načini se prevedejo,
                        // vendar se le za lambda izraz filtriranje izvede na podatkovni strukturi,
                        // kjer delamo poizvedbo.
                        // Za druga dva načina se podatki najprej prenesejo
                        // k nam in filtrirajo na naši strani, 
                        // kar lahko prinese velike časovne razlike.
                        // Na to moramo torej paziti, ko implementiramo svoje metode, 
                        // ki delajo poizvedbe na podatkovni bazi - vračanje IQueryable<T> ali IEnumerable<T> 
                        // lahko bistveno upočasni našo aplikacijo. [Arh, Q58]
                    }
                    break;
            }
        }

        /// <summary>
        /// Pri definiciji te funkcije smo uporabili 
        /// posebno skrajšano sintakso za funkcije, ki imajo samo return stavek.
        /// </summary>
        private static bool LessThan4(Animal animal) => animal.NumberOfLegs < 4;

        private static bool LessThan4Legs(Animal animal)
        {
            return animal.NumberOfLegs < 4;
        }
    }
}
