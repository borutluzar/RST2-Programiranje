using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace DesignPatterns
{
    class SingletonTests
    {
        public static void CreateSingleton()
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

        public static void CreateLog()
        {
            EventLog log = EventLog.Instance();

            log.WriteEvent("Kreiramo nov dogodek.");
            Thread.Sleep(1000);
            log.WriteEvent("Kreiramo še enega.");
            Thread.Sleep(1000);
            log.WriteEvent("Počasi zaključujemo program.");

            Console.WriteLine("Konec zapisovanja v dnevnik");

            // Čez dva dni v isti funkciji
            // spet pokličemo instanco
            Thread.Sleep(2000);

            EventLog log2 = EventLog.Instance();
            log2.WriteEvent("Kreiramo nov dogodek - tadabum!");
        }
    }

    /// <summary>
    /// Če želimo zagotoviti, da bomo imeli v našem programu zgolj oziroma največ eno instanco 
    /// izbranega razreda, uporabimo vzorec singleton.
    /// Ideja singletona je, da ima izbrani razred zgolj privaten konstruktor, 
    /// namesto konstruktorja pa razred izpostavi javno statično metodo, 
    /// ki skrbi za instanco razreda.
    /// 
    /// Ker pa gre za razred, ki ni statičen, lahko deduje lastnosti in metode nekega razreda 
    /// ali implementira vmesnike!
    /// </summary>
    public sealed class Singleton
    {
        /// <summary>
        /// Instanca, ki nadzoruje obstoj instance
        /// </summary>
        private static Singleton uniqueInstance = null;

        /// <summary>
        /// Konstruktor, do katerega dostopamo samo znotraj razreda,
        /// brez parametrov
        /// </summary>
        private Singleton()
        {
            // Vsaki instanci priredimo naključni ID
            Random rnd = new Random();
            this.RandomID = rnd.Next(1, 101);
        }

        /// <summary>
        /// Javna (objektna!) lastnost vsake instance tega razreda
        /// </summary>
        public int RandomID { get; }

        /// <summary>
        /// Javna metoda, ki poskrbi za kreiranje instance,
        /// če še ne obstaja in jo vrne.
        /// </summary>
        /// <returns>Vrača edino instanco razreda</returns>
        public static Singleton Instance()
        {
            // Če instanca še ni bila inicializirana, pokličemo konstruktor
            if (uniqueInstance == null)
                uniqueInstance = new Singleton();

            return uniqueInstance;
        }
    }


    /**
     * Dodatno branje: 
     * - https://csharpindepth.com/articles/singleton
     * - https://dotnettutorials.net/lesson/singleton-design-pattern/
     * 
     * Skupne lastnosti vzorca singleton:
     * - Razred ima privaten konstruktor brez parametrov
     * - Razred ima določilo 'sealed', kar pomeni, da se ga ne more dedovati 
     * - Ima statično spremenljivko, ki nosi referenco do morebitne edine instance razreda
     * - Ima javno statično metodo, ki to edino instanco posreduje klicatelju
     * 
     * Razred singleton lahko deduje od drugih razredov in implementira vmesnike, 
     * zato ima prednost pred statičnimi razredi.
     * Njegova slabost se pokaže pri uporabi paralelizacije, ko hkrati dostopamo do edine instance.
     * V takem primeru jo moramo ob dostopu zakleniti ('lock').
     * 
     * Nekaj primerov uporabe:
     * - Razred za kontroliranje dnevniških (log) zapisov v datoteko
     * - Povezava na podatkovno bazo
     * - Pomnenje "fiksnih" vrednosti iz podatkovne baze, ki se ob izvajanju programa ne spreminjajo ("Caching") 
     */


    /// <summary>
    /// Primer uporabe singletona za razred, ki beleži dnevniške zapise v eno datoteko.
    /// </summary>    
    public sealed class EventLog
    {
        private static EventLog instance = null;

        private EventLog()
        {
            string folderName = ""; // Ime mape, kamor shranjujemo zapise, pridobimo iz baze ali konfiguracijske datoteke
            DateTime dtNow = DateTime.Now;

            // Prepare new file for logging
            this.LogFile = $"{folderName}EventLog-{dtNow:yyyy-MM-dd_HH-mm-ss}.txt";
        }

        private string LogFile { get; }

        public void WriteEvent(string evnt)
        {
            DateTime dtNow = DateTime.Now;
            StreamWriter sw = new StreamWriter(this.LogFile, true, Encoding.UTF8);
            sw.WriteLine($"Event at {dtNow:HH-mm-ss}");
            sw.WriteLine(evnt);
            sw.WriteLine();
            sw.Close();
        }

        /// <summary>
        /// Statična funkcija, ki vrača edino instanco razreda.
        /// </summary>
        public static EventLog Instance()
        {
            if (instance == null)
            {
                instance = new EventLog();
            }

            return instance;
        }
    }
}
