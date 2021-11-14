using CommonFunctions;
using System;
using System.Threading;

namespace DesignPatterns
{
    class Program
    {
        enum DesignPatternsSections
        {
            Singleton = 1,
            SingletonLog = 2,
            FactoryBad = 3,
            FactoryGood = 4
        }

        static void Main()
        {
            switch (InterfaceFunctions.ChooseSection<DesignPatternsSections>())
            {
                case DesignPatternsSections.Singleton:
                    {
                        // Osnoven primer kreiranja singletona
                        SingletonTests.CreateSingleton();
                    }
                    break;
                case DesignPatternsSections.SingletonLog:
                    {
                        // Primer uporabe singletona za kreiranje objekta log (dnevnik dogodkov)
                        SingletonTests.CreateLog();
                    }
                    break;
                case DesignPatternsSections.FactoryBad:
                    {
                        // Slab primer izbire tipa kartice:

                        // Tip kartice se izbere v GUI-ju
                        CreditCardType type = InterfaceFunctions.ChooseSection<CreditCardType>();

                        // Pripravimo si novo spremenljivko
                        ICreditCard card = null;

                        // Ustvarimo instanco glede na izbrani tip
                        switch (type) 
                        {
                            case CreditCardType.Silver:
                                card = new Silver();
                                break;
                            case CreditCardType.Gold:
                                card = new Gold();
                                break;
                            case CreditCardType.Platinum:
                                card = new Platinum();
                                break;
                        }

                        // Izpišemo podatke
                        Console.WriteLine("Podatki o kartici:");
                        Console.WriteLine($"  Tip: {card.CreditCardType}");
                        Console.WriteLine($"  Limit: {card.Limit}");
                        Console.WriteLine($"  Letni strošek: {card.AnnualCharge}");

                        /*
                         * Težava pri zgornjem načinu je, da so uporabniški vmesnik in razredi kreditnih kartic
                         * močno povezani. Če dodamo nov tip kartice, moramo to popraviti tudi v uporabniškem vmesniku,
                         * saj je treba dopolniti switch.
                         */ 
                    }
                    break;
                case DesignPatternsSections.FactoryGood:
                    {
                        // V uporabniškem vmesniku ohranimo samo logiko, ki se tiče uporabnika
                        CreditCardType type = InterfaceFunctions.ChooseSection<CreditCardType>();
                        ICreditCard card = CreditCardFactory.GetCreditCard(type);

                        // Izpišemo podatke
                        Console.WriteLine("Podatki o kartici:");
                        Console.WriteLine($"  Tip: {card.CreditCardType}");
                        Console.WriteLine($"  Limit: {card.Limit}");
                        Console.WriteLine($"  Letni strošek: {card.AnnualCharge}");
                    }
                    break;
            }
            Console.ReadLine();
        }
    }
}
