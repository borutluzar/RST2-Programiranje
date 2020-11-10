using System;
using System.Threading;

namespace NacrtovalskiVzorci
{
    class Program
    {
        enum Section
        {
            Singleton,
            SingletonLog,
            FactoryBad,
            FactoryGood
        }

        static void Main(string[] args)
        {
            Section section = Section.Singleton;

            Console.WriteLine(); // Za prostor okoli okvirja
            switch (section)
            {
                case Section.Singleton:
                    {                     
                        // Klic konstruktorja se ne prevede
                        //Singleton single = new Singleton();
                        
                        // Pokličimo funkcijo enkrat
                        Singleton single1 = Singleton.Instance();
                        Console.WriteLine($"Naključni ID prve instance je: {single1.RandomID}");

                        // In ponovno
                        Singleton single2 = Singleton.Instance();
                        Console.WriteLine($"Naključni ID druge instance je: {single2.RandomID}");
                    }
                    break;
                case Section.SingletonLog:
                    {
                        EventLog log = EventLog.Instance();
                        log.WriteEvent("Kreiramo nov dogodek.");
                        Thread.Sleep(1000);
                        log.WriteEvent("Kreiramo še enega.");
                        Thread.Sleep(1000);
                        log.WriteEvent("Počasi zaključujemo program.");
                        
                        Console.WriteLine("Konec zapisovanja v dnevnik");
                    }
                    break;
                case Section.FactoryBad:
                    {
                        // Tip kartice se izbere v GUI-ju
                        CreditCardType type = CreditCardType.Silver;

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
                case Section.FactoryGood:
                    {
                        // V uporabniškem vmesniku ohranimo samo logiko, ki se tiče uporabnika
                        CreditCardType type = CreditCardType.Silver;
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
