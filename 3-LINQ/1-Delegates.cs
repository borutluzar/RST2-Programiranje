using CommonFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LINQ
{
    /// <summary>
    /// Delegati določijo/definirajo sklice na metode z danim seznamom tipov parametrov in tipom vračanja.
    /// Z njimi si pomagamo, ko želimo zapisati metodo, kateri kot parameter na enak način dodamo 
    /// različne funkcije z istim podpisom (seznam vhodnih in izhodnih tipov parametrov).
    /// 
    /// Več o delegatih in njihovi uporabi najdete v [Arh, Q29]
    /// </summary>
    public class Delegates
    {
        enum DelegateSubsections
        {
            Definition = 1,
            MultipleFunctionReferences = 2,
            DelegatesAsParameters = 3,
            NetSimplification = 4,
            Reflections
        }

        public static void DelegateExamples()
        {
            switch (InterfaceFunctions.ChooseSection<DelegateSubsections>())
            {
                case DelegateSubsections.Definition:
                    {
                        // Pripravimo instanco spodaj pripravljenega delegata NumberProperty
                        // in mu dajmo sklic na funkcijo IsPrimeNumber
                        NumberProperty property = IsPrimeNumber;
                        NumberProperty property2 = IsOdd;

                        // Poglejmo izpis
                        int input = 2;
                        var result = property(input);
                        Console.WriteLine($"\nSpremenljivka {nameof(property)} ima vrednost {property.Method.Name}");
                        Console.WriteLine($"Za število {input} lastnost {(result ? "drži" : "ne drži")}");


                        // Zamenjajmo vrednost
                        property = IsOdd;
                        result = property(input);
                        Console.WriteLine($"\nSpremenljivka {nameof(property)} ima vrednost {property.Method.Name}");
                        Console.WriteLine($"Za število {input} lastnost {(result ? "drži" : "ne drži")}");
                    }
                    break;
                case DelegateSubsections.MultipleFunctionReferences:
                    {
                        // Posameznemu delegatu lahko pripišemo več funkcij,
                        // vendar moramo biti pozorni na to, da vračajo samo vrednost
                        // zadnje klicane funkcije.
                        // Zato jim je na tak način smiselno pripisovati void funkcije (Actions)
                        NumberProperty property = IsPrimeNumber;
                        int input = 121;

                        // Dodajmo še nekaj funkcij
                        property += IsOdd;
                        property += IsDivisibleBy11;
                        // Te funkcije se zbirajo v seznamu GetInvocationList
                        Console.WriteLine($"\nSpremenljivka {nameof(property)} se sklicuje na funkcije " +                            
                            $"{property.GetInvocationList().Select(x => x.Method.Name).ToList().ToString<string>()}, " +
                            $"zadnja funkcija pa je {property.Method.Name}");
                        Console.WriteLine($"Za število {input} lastnost {(property(input) ? "drži" : "ne drži")}");
                    }
                    break;
                case DelegateSubsections.DelegatesAsParameters:
                    {
                        // Ključna uporaba delegatov je v primeru njihovega podajanja kot parametrov
                        // funkcijam, da funkcije s tem naredimo bolj splošne.

                        List<int> lstInputs = new() { 3, 5, 7, 12, 13, 14, 18, 23, 25, 28, 43, 55, 56, 57, 58, 82, 85, 89 };
                                                

                        // Pokličimo funkcijo CheckProperty za različne lastnosti
                        var lstResults = CheckProperty(lstInputs, IsPrimeNumber);
                        lstResults.ForEach(x => Console.WriteLine(x.ToString()));
                    }
                    break;
                case DelegateSubsections.NetSimplification:
                    {
                        // V ogrodju .Net je definiranje delegatov precej poenostavljeno.
                        // Na primer, samo definiranje kratke metode, ki izvede preprost izračun
                        // nima velikega pomena, enostavneje je ta izračun zapisati neposredno.
                        // Tudi posebno definiranje delegatov je običajno pretirano.
                        // V ta namen imamo na voljo tri pred-pripravljene tipe:
                        // Action<> - uporabimo ga, ko izvedemo akcijo z danimi parametri, vendar ne vračamo ničesar,
                        // Func<> - uporabimo, ko za dani parameter izračunamo in vrnemo rezultat in
                        // Predicate<> - je posebna vrsta tipa Func<>, ki kot rezultat vedno vrača boolean.

                        // Izvedimo preverjanje parametrov kot v prejšnjem primeru:
                        List<int> lstInputs = new() { 3, 5, 7, 12, 13, 14, 18, 23, 25, 28, 43, 55, 56, 57, 58, 82, 85, 89 };

                        // Predikati vedno vračajo bool
                        Predicate<int> property = IsPrimeNumber;

                        // Pokličimo funkcijo CheckProperty s predikatom
                        var lstResults = CheckProperty2(lstInputs, property);
                        lstResults.ForEach(x => Console.WriteLine(x.ToString()));

                        // Na enak način bi lahko definirali Func<>
                        Func<int, bool> propertyFunc = IsPrimeNumber;

                        // Če imamo več vhodnih parametrov, njihove tipe naštejemo po vrsti,
                        // zadnji je tip rezultata.
                        Func<int, int, int> funcMax = Math.Max;
                        // Običajno tipe Func, Action in Predicate zapisujemo kar v obliki lambda izrazov...
                    }
                    break;
                case DelegateSubsections.Reflections:
                    {
                        // Včasih želimo metodo poklicati na podlagi njenega imena.
                        // Npr., ime metode preberemo iz baze ali iz danega seznama.
                        // Pridobimo jih (javne!) s pomočjo paketa System.Reflection

                        // Najprej povemo, v katerem razredu iščemo metode
                        Type type = typeof(Delegates);  // ali: Type.GetType("LINQ.Delegates"); 
                        Console.WriteLine($"Javne metode v razredu {type.Name} " +
                            $"so: {type.GetMethods().Select(m => m.Name).ToString<string>()}");

                        // Pridobimo željeno metodo
                        MethodInfo method = type.GetMethod("IsOdd");
                        // Priredimo vrednost delegatu
                        NumberProperty property = Delegate.CreateDelegate(typeof(NumberProperty), method) as NumberProperty;

                        int input = 10;
                        Console.WriteLine($"Za število {input} lastnost {property.Method.Name} {(property(input) ? "drži" : "ne drži")}");
                    }
                    break;
            }
        }


        // Definirajmo delegata, ki bo predstavljal funkcijo,
        // katere vhodni podatek je celo število, vrača pa boolean.
        //
        // Ker je delegat sklic na funkcijo, se držimo konvencije
        // in ga označimo z veliko začetnico.
        public delegate bool NumberProperty(int n);

        // Pripravimo si še nekaj funkcij, ki jih bomo podajali kot
        // vrednost delegata.
        public static bool IsPrimeNumber(int n)
        {
            Console.WriteLine("\nPoteka izvajanje funkcije IsPrimeNumber...");

            for (int i = 2; i <= (int)Math.Sqrt(n); i++)
                if (n % i == 0)
                    return false;

            return true;
        }

        public static bool IsOdd(int n)
        {
            Console.WriteLine("\nPoteka izvajanje funkcije IsOdd...");
            return n % 2 == 1;
        }

        public static bool IsDivisibleBy11(int n)
        {
            Console.WriteLine("\nPoteka izvajanje funkcije IsDivisibleBy11...");
            return n % 11 == 0;
        }


        // Definirajmo funkcijo, ki kot parameter prejme seznam števil in delegata za preverjanje izbrane lastnosti.
        public static List<(int, bool)> CheckProperty(List<int> lstNumbers, NumberProperty property)
        {
            List<(int, bool)> lstChecked = new List<(int, bool)>();
            foreach (int n in lstNumbers)
            {
                lstChecked.Add((n, property(n)));
            }
            return lstChecked;
        }

        /// <summary>
        /// Namesto posebnega delegata, lahko funkciji kot parameter podamo pred-pripravljen tip Predicate.
        /// </summary>
        public static List<(int, bool)> CheckProperty2(List<int> lstNumbers, Predicate<int> property)
        {
            List<(int, bool)> lstChecked = new List<(int, bool)>();
            foreach (int n in lstNumbers)
            {
                lstChecked.Add((n, property(n)));
            }
            return lstChecked;
        }
    }
}
