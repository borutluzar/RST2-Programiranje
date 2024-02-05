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

            Employee researcher = new PublicRelationsPerson("Borut", "Xy");
            researcher.PaySalary(8000, "201314-1201");
            researcher.TrySpeakForeignLanguage("mandarinščina");
            researcher.TryMonitorExam();
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

        public string Gender { get; set; }

        /// <summary>
        /// Vsak zaposleni dobi plačo
        /// </summary>
        public void PaySalary(int amount, string bankAccount)
        {
            // Nakaže plačo na osebni račun
            Console.WriteLine($"Vaša plača je bila izplačana danes, {DateTime.Now:d. M. yyyy}, na račun številka {bankAccount}");
        }

        // Metodo SpeakForeignLanguage nadomestimo z instanco vmesnika IForeignLanguageSpeaker
        // Tukaj smo uporabili strateški vzorec, saj za vsako spreminjajočo se funkcionalnost
        // implementiramo poseben vmesnik, za implementacijo metod vmesnika pa pripravimo posebne razrede,
        // katerih instance ustvarimo v konstruktorjih podrazredov.
        protected IForeignLanguageSpeaker SpeakForeign { get; set; }

        /// <summary>
        /// In pripravimo metodo, ki kliče metodo ustrezne instance IForeignLanguageSpeaker
        /// </summary>
        public void TrySpeakForeignLanguage(string language)
        {
            SpeakForeign.SpeakForeignLanguage(language);
        }


        // Sklop za akademike
        protected IAcademicPerformer AcademicPerformer { get; set; }

        public int GetHIndex()
        {
            return AcademicPerformer.HIndex;
        }


        // Sklop za izpite
        protected IExamination Examination { get; set; }

        public void TryMonitorExam()
        {
            if (Examination != null)
                Examination.MonitorExam("Programiranje");
            else
                Console.WriteLine("Tega ni v mojem opisu del in nalog.");
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
            // Tukaj določimo ustrezno instanco vmesnika IForeignLanguageSpeaker za ta razred
            SpeakForeign = new SpeakForeignLanguageFluently();
            // In podobno za instanco vmesnika IAcademicPerformer
            AcademicPerformer = new AcademicInNaturalSciences();

            Examination = new ExaminationPerson();
        }

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
            // Tukaj določimo ustrezno instanco vmesnika IForeignLanguageSpeaker za ta razred
            SpeakForeign = new SpeakForeignLanguageSoSo();

            // In podobno za instanco vmesnika IAcademicPerformer
            AcademicPerformer = new AcademicInSocialSciences();

            Examination = new ExaminationPerson();
        }

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
            // Tukaj določimo ustrezno instanco vmesnika IForeignLanguageSpeaker za ta razred
            SpeakForeign = new SpeakForeignLanguageFluently();

            // In podobno za instanco vmesnika IAcademicPerformer
            AcademicPerformer = new NotAcademic();

            Examination = null;
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
            // Tukaj določimo ustrezno instanco vmesnika IForeignLanguageSpeaker za ta razred
            SpeakForeign = new SpeakForeignLanguageNot();

            // In podobno za instanco vmesnika IAcademicPerformer
            AcademicPerformer = new NotAcademic();

            Examination = null;
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

        void ReadForeignLanguage(string language);
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

        public void ReadForeignLanguage(string language)
        {
            Console.WriteLine($"V tujem jeziku izvrstno berem!");
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

        public void ReadForeignLanguage(string language)
        {
            Console.WriteLine($"V tujem jeziku berem bolje kot pišem!");
        }
    }

    /// <summary>
    /// Razred, ki implementira metodo za negovorjenje tujega jezika.
    /// </summary>
    public class SpeakForeignLanguageNot : IForeignLanguageSpeaker
    {
        public void SpeakForeignLanguage(string language)
        {
            Console.WriteLine($"Tega jezika ne govorim.");
        }

        public void ReadForeignLanguage(string language)
        {
            Console.WriteLine($"V tujem jeziku berem, ampak nič ne razumem!");
        }
    }

    public interface IAcademicPerformer
    {
        public int HIndex { get; }
    }

    public class AcademicInNaturalSciences : IAcademicPerformer
    {
        public int HIndex
        {
            get
            {
                return 12; // After rigorous computations
            }
        }
    }

    public class AcademicInSocialSciences : IAcademicPerformer
    {
        public int HIndex
        {
            get
            {
                return 4; // After even more rigorous computations
            }
        }
    }

    public class NotAcademic : IAcademicPerformer
    {
        public int HIndex
        {
            get
            {
                return 0;
            }
        }
    }


    public interface IExamination
    {
        public string PrepareExamQuestions(string subject);

        public void MonitorExam(string subject);

        public void GradeExams(string subject);
    }

    public class ExaminationPerson : IExamination
    {
        public string PrepareExamQuestions(string subject)
        {
            return $"Vprašanja za izpit is {subject} so naslednja: Kdo?";
        }

        public void MonitorExam(string subject)
        {
            Console.WriteLine($"Pazimo izpit is {subject}.");
        }

        public void GradeExams(string subject)
        {
            Console.WriteLine($"Vsi so naredili.");
        }
    }
}
