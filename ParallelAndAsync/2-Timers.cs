using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;

namespace ParallelAndAsync
{
    /// <summary>
    /// Arh, Q60
    /// Štoparice (timers) uporabljamo, 
    /// ko nek proces izvajamo v danem intervalu oziroma
    /// po preteku določenega časa.
    /// </summary>
    class Timers
    {
        private static readonly string stateObject = "Borut";

        public static void TimersTest()
        {

            Console.WriteLine("Smo pred zagonom procesa v timerju...");

            // Razred System.Threading.Timer uporabimo, ko želimo sprožiti akcijo
            // po določenem času.
            // Vse potrebne parametre določimo že v konstruktorju.
            // POZOR! Timer razredov je več, zato določimo natančno mesto.
            var timer = new System.Threading.Timer(
                state => {
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss}: Uporabnik {state} kliče kodo v timerju!"); // Dejanska koda, ki se izvede
                }, 
                stateObject, // Objekt, ki ga uporabimo v spremenljivki state v zgornjem lambda izrazu
                3000,   // Zamik pred prvim klicem timerja
                Timeout.Infinite    // Interval med zaporednimi klici. V tem primeru naslednjega klica ne bo.
                );

            // Izpis se izpiše takoj, medtem ko se na izvedbo kode v timerju še malo počaka.
            // Koda timerja se izvede v niti, ki bo prosta. Ni nujno, da bo to trenutna nit.
            Console.WriteLine("In to je izpis po kodi, ki določa timer.");

            // Spremenimo še intervale z metodo Change
            Console.WriteLine("Sprememba intervalov v timerju.");
            timer.Change(5000, 1000);

            // Kaj naredi spodnja koda?
            var stopper = new System.Threading.Timer(
                state => { timer.Change(1000, Timeout.Infinite); }, null, 10000, Timeout.Infinite);

            // Za zaustavitev izvajanja timerja lahko uporabimo tudi klic timer.Dispose() na ustreznem mestu.
        }
    }
}
