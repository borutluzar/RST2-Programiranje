using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izpit_2023_02_10
{
    public abstract class Disciplina
    {
        public string Naziv { get; }

        protected Disciplina(string naziv) 
        {
            this.Naziv = naziv;
        }

        public int KazenskeTocke { get; set; }

        public int CasIzvedbe { get; set; }

        public int SkupneTocke => this.CasIzvedbe + this.KazenskeTocke;

        public abstract int PrestejKazenskeTocke();
    }

    public class Razvrscanje : Disciplina
    {
        public Razvrscanje() : base("Razvrščanje") { }

        public override int PrestejKazenskeTocke()
        {
            throw new NotImplementedException();
        }
    }

    public class Vozli : Disciplina
    {
        public Vozli() : base("Vozli") { }

        public override int PrestejKazenskeTocke()
        {
            throw new NotImplementedException();
        }
    }

    public class TrodelniNapad : Disciplina
    {
        public TrodelniNapad() : base("Trodelni napad") { }

        public override int PrestejKazenskeTocke()
        {
            throw new NotImplementedException();
        }

        public void Izvedi()
        {
            Random rnd = new Random();
            int pause = rnd.Next(2500, 3500);
            Thread.Sleep(pause);            
        }
    }

    public class Raznoternost : Disciplina
    {
        public Raznoternost() : base("Raznoternost") { }

        public override int PrestejKazenskeTocke()
        {
            throw new NotImplementedException();
        }
    }

    public class Stafeta : Disciplina
    {
        public Stafeta() : base("Štafeta") { }

        public override int PrestejKazenskeTocke()
        {
            throw new NotImplementedException();
        }
    }
}
