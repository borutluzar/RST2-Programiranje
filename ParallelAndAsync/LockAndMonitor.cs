using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ParallelAndAsync
{
    /// <summary>
    /// Ko z eno spremenljivko dela več niti hkrati,
    /// moramo paziti, da lahko vanjo zapisuje le ena nit.
    /// Najbolj enostaven način, da to dosežemo, je
    /// stavek lock (sinhronizacijski primitiv).
    /// </summary>
    class LockAndMonitor
    {
        public static void LockExample()
        {
            BankAccount myAccount = new BankAccount() { Balance = 20000 };

            List<double> amounts = new List<double>();
            for (int i = 0; i < 100000; i++)
            {
                amounts.Add(1);
            }

            // Če objekt zaklenemo, bo rezultat pravilen, kljub hkratnemu brisanju z računa
            amounts.AsParallel().ForAll(a => myAccount.WithdrawWithLock(a));

            // Brez lock-a tega ne moremo zagotoviti
            //amounts.AsParallel().ForAll(a => myAccount.WithdrawNoLock(a));

            /*for (int i = 0; i < 20; i++)
            {
                amounts.AsParallel().ForAll(a => myAccount.WithdrawNoLock(a));
                Console.WriteLine($"Stanje na računu je: {myAccount.Balance}");
                myAccount.Balance = 20000;
            }*/

            Console.WriteLine($"Stanje na računu je: {myAccount.Balance}");

            // Daljša oblika sintakse lock je uporaba objekta Monitor.
            // V osnovi stori enako kot lock, zato ga ni smiselno uporabiti, 
            // če ne nameravamo uporabljati drugih funkcionalnosti, ki jih
            // omogoča. Npr. spremljanje, kako dolgo je objekt zaklenjen.
            // Če  se npr. zgodi deadlock, bo aplikacija obvisela, 
            // če za to ne poskrbimo posebej.
        }

    }

    class BankAccount
    {
        public double Balance { get; set; }

        public bool WithdrawWithLock(double amount)
        {
            lock (this)
            {
                if (amount > this.Balance)
                {
                    return false;
                }
                this.Balance -= amount;
                return true;
            }
        }

        public bool WithdrawNoLock(double amount)
        {
            if (amount > this.Balance)
            {
                return false;
            }
            this.Balance -= amount;
            return true;
        }
    }
}
