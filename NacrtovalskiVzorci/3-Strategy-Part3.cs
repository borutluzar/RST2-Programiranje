using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.StrategyPart3
{
    /** 
     * Strateški načrtovalski vzorec spada med vedenjske. 
     * To pomeni, da nam pomaga pri implementaciji "vedenja" posameznih objektov,
     * ki navadno spadajo v kak širši razred.
     * 
     * Povzeto po knjigi:
     * E. Freeman et al. (2004), Head First Design Patterns, O'Reilly Media, USA. 
     */

    public class Part3Test
    {
        public static void ShowPart3()
        {
            // Specifikacije razrednega modela se pri "živih" aplikacijah nenehno spreminjajo,
            // zato si želimo čim bolj fleksibilnega modela, ki ga bo enostavno dopolnjevati na eni strani
            // in se mu koda ne podvaja na drugi strani.

            // V tem delu smo razredoma Researcher in Lecturer dodali lastnost HIndex
            // (samo njima), kar pomeni, da smo kodo (npr. implementacijo izračuna indeksa) podvojili.
            // Dodali smo še tip Janitor, ki ne zna tujega jezika, kar pomeni,
            // da smo v njem morali povoziti metodo za govorjenje tujega jezika in
            // ji umakniti funkcionalnost, kar je spet slaba praksa.
        }
    }


    /// <summary>
    /// Temeljni razred, katerega vejimo na nekaj razredov 
    /// različnih tipov uslužbencev z različnimi znanji.
    /// </summary>
    public abstract class Employee
    {
        public Employee(string familyName, string givenName)
        {
            FamilyName = familyName;
            GivenName = givenName;
        }

        public string FamilyName { get; set; }
        public string GivenName { get; set; }

        /// <summary>
        /// Vsak zaposleni dobi plačo
        /// </summary>
        public void PaySalary(int amount, string bankAccount)
        {
            // Pay salary to personal account...
            Console.WriteLine($"Vaša plača je bila izplačana danes, {DateTime.Now:d. M. yyyy}, na račun številka {bankAccount}");
        }

        // Metodo SpeakForeignLanguage nadomestimo z instanco vmesnika IForeignLanguageSpeaker
        protected IForeignLanguageSpeaker SpeakForeign { get; set; }

        /// <summary>
        /// Pripravimo metodo, ki kliče metodo ustrezne instance
        /// </summary>
        public void TrySpeakForeignLanguage(string language)
        {
            SpeakForeign.SpeakForeignLanguage(language);
        }

        public virtual void WorkDuties()
        {
            // Vsi zaposleni imajo neke skupne zadolžitve
            Console.WriteLine($"Paziti moram, da ne zanetim požara!");
        }
    }

    // Zaposlene delimo na tri podrazrede, vsak od njih ima nekaj posebnosti.

    public class Researcher : Employee
    {
        public Researcher(string familyName, string givenName) : base(familyName, givenName) 
        {
            SpeakForeign = new SpeakForeignLanguageFluently();
        }

        public int HIndex { get; set; }

        public override void WorkDuties()
        {
            // Vsak poseben tip zaposlenega pa ima še posebne zadolžitve
            base.WorkDuties();
            Console.WriteLine($"Tudi raziskovati moram.");
        }

        /// <summary>
        /// Posebna metoda, ki jo ima le ta tip.
        /// </summary>
        public void WriteProjectApplication()
        {
            Console.WriteLine($"Pridno pišem raziskovalna vprašanja.");
        }
    }

    public class Lecturer : Employee
    {
        public Lecturer(string familyName, string givenName) : base(familyName, givenName) 
        {
            SpeakForeign = new SpeakForeignLanguageSoSo();
        }

        public int HIndex { get; set; }

        public override void WorkDuties()
        {
            // Vsak poseben tip zaposlenega pa ima še posebne zadolžitve
            base.WorkDuties();
            Console.WriteLine($"Učim. Preveč.");
        }

        /// <summary>
        /// Posebna metoda, ki jo ima le ta tip.
        /// </summary>
        public void PerformExamination()
        {
            Console.WriteLine($"Pridno sprašujem študente.");
        }
    }

    public class PublicRelationsPerson : Employee
    {
        public PublicRelationsPerson(string familyName, string givenName) : base(familyName, givenName) 
        {
            SpeakForeign = new SpeakForeignLanguageFluently();
        }

        public override void WorkDuties()
        {
            // Vsak poseben tip zaposlenega pa ima še posebne zadolžitve
            base.WorkDuties();
            Console.WriteLine($"Ukvarjam se s trženjem");
        }

        /// <summary>
        /// Posebna metoda, ki jo ima le ta tip.
        /// </summary>
        public void MakeCampaign()
        {
            Console.WriteLine($"Pridno pripravljam reklamno kampanjo.");
        }
    }

    public class Janitor : Employee
    {
        public Janitor(string familyName, string givenName) : base(familyName, givenName) 
        {
            SpeakForeign = new SpeakForeignLanguageNot();
        }

        public override void WorkDuties()
        {
            // Vsak poseben tip zaposlenega pa ima še posebne zadolžitve
            base.WorkDuties();
            Console.WriteLine($"Popravim vse, kar se pokvari.");
        }

        /// <summary>
        /// Posebna metoda, ki jo ima le ta tip.
        /// </summary>
        public void MoweLawn()
        {
            Console.WriteLine($"Kosim trato.");
        }
    }

    // Strateški načrtovalski vzorec nam svetuje, da lastnosti in funkcionalnosti,
    // katere ima samo neka množica podrazredov zabeležimo v vmesnikih in zanje
    // naredimo IMPLEMENTACIJE V POSEBNIH RAZREDIH.

    /// <summary>
    /// Vmesnik, ki zagotovi metode za govorjenje tujega jezika.
    /// </summary>
    public interface IForeignLanguageSpeaker
    {
        void SpeakForeignLanguage(string language);
    }

    /// <summary>
    /// Razred, ki implementira metodo za tekoče govorjenje tujega jezika.
    /// </summary>
    public class SpeakForeignLanguageFluently : IForeignLanguageSpeaker
    {
        public void SpeakForeignLanguage(string language)
        {
            Console.WriteLine($"Tuj jezik {language} govorim tekoče dva dni skupaj!");
        }
    }

    /// <summary>
    /// Razred, ki implementira metodo za manj tekoče govorjenje tujega jezika.
    /// </summary>
    public class SpeakForeignLanguageSoSo : IForeignLanguageSpeaker
    {
        public void SpeakForeignLanguage(string language)
        {
            Console.WriteLine($"Tuj jezik {language} govorim bolj tako tako.");
        }
    }

    /// <summary>
    /// Razred, ki implementira metodo za negovorjenje tujega jezika.
    /// </summary>
    public class SpeakForeignLanguageNot : IForeignLanguageSpeaker
    {
        public void SpeakForeignLanguage(string language)
        {
            Console.WriteLine($"Nič ne bo.");
        }
    }
}
