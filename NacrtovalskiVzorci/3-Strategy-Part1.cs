using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.StrategyPart1
{
    /** 
     * Strateški načrtovalski vzorec spada med vedenjske. 
     * To pomeni, da nam pomaga pri implementaciji "vedenja" posameznih objektov,
     * ki navadno spadajo v kak širši razred.
     * 
     * Povzeto po knjigi:
     * E. Freeman et al. (2014), Head First Design Patterns, O'Reilly Media, USA. 
     */

    public class Part1Test
    {
        public static void ShowPart1()
        {
            // Specifikacije razrednega modela se pri "živih" aplikacijah nenehno spreminjajo,
            // zato si želimo čim bolj fleksibilnega modela, ki ga bo enostavno dopolnjevati na eni strani
            // in se mu koda ne podvaja na drugi strani.
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
            // Plačo izplačamo na osebni račun
            Console.WriteLine($"Vaša plača (skupno {amount}€) je bila izplačana danes, {DateTime.Now:d. M. yyyy}, na račun številka {bankAccount}");
        }

        /// <summary>
        /// Vsak zaposleni govori tuj jezik
        /// </summary>
        public void SpeakForeignLanguage(string language)
        {
            Console.WriteLine($"Res je, govorim {language}");
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
        public Researcher(string familyName, string givenName) : base(familyName, givenName) { }

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
        public Lecturer(string familyName, string givenName) : base(familyName, givenName) { }

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
        public PublicRelationsPerson(string familyName, string givenName) : base(familyName, givenName) { }

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
}
