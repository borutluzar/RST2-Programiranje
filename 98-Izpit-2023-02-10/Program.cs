namespace Izpit_2023_02_10
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Izvajanje programa za prvi izpit iz Programiranja z dne 10. 2. 2023!");

            Tekmovanje fisPrvenstvo = new("Prvenstvo FIŠ", "Novo mesto", 2024);
            
            Kategorija katClanice = new(KategorijaEnum.Člani, Spol.Ženske);
            Kategorija katClani = new(KategorijaEnum.Člani, Spol.Moški);
            fisPrvenstvo.Kategorije.Add(katClanice);
            fisPrvenstvo.Kategorije.Add(katClani);

            Ekipa ek0 = new("Prvakinje", "GD Novo mesto", Spol.Ženske);
            Ekipa ek1 = new("Prvaki", "GD Novo mesto", Spol.Moški);
            Ekipa ek2 = new("Atletinje", "GD Mirna Peč", Spol.Ženske);
            Ekipa ek3 = new("Atleti", "GD Mirna Peč", Spol.Moški);
            Ekipa ek4 = new("Piromani", "GD Ljubljana", Spol.Moški);

            katClanice.Ekipe.Add(ek0);
            katClanice.Ekipe.Add(ek2);

            katClani.Ekipe.Add(ek1);
            katClani.Ekipe.Add(ek3);
            katClani.Ekipe.Add(ek4);

            // Dodajmo člane
            Disciplina discRazvM = new Razvrscanje();
            Disciplina discTrodelniM = new TrodelniNapad();
            katClani.Discipline.Add(discRazvM);
            katClani.Discipline.Add(discTrodelniM);

            ek1.Discipline.Add(new Razvrscanje() { KazenskeTocke = 13, CasIzvedbe = 0 });
            ek1.Discipline.Add(new TrodelniNapad() { KazenskeTocke = 5, CasIzvedbe = 52 });

            ek3.Discipline.Add(new Razvrscanje() { KazenskeTocke = 43, CasIzvedbe = 0 });
            ek3.Discipline.Add(new TrodelniNapad() { KazenskeTocke = 45, CasIzvedbe = 122 });

            ek4.Discipline.Add(new Razvrscanje() { KazenskeTocke = 0, CasIzvedbe = 0 });
            ek4.Discipline.Add(new TrodelniNapad() { KazenskeTocke = 15, CasIzvedbe = 48 });

            // Dodajmo članice
            Disciplina discRazvZ = new Razvrscanje();
            Disciplina discTrodelniZ = new TrodelniNapad();
            katClanice.Discipline.Add(discRazvZ);
            katClanice.Discipline.Add(discTrodelniZ);

            ek0.Discipline.Add(new Razvrscanje() { KazenskeTocke = 12, CasIzvedbe = 0 });
            ek0.Discipline.Add(new TrodelniNapad() { KazenskeTocke = 5, CasIzvedbe = 53 });

            ek2.Discipline.Add(new Razvrscanje() { KazenskeTocke = 10, CasIzvedbe = 0 });
            ek2.Discipline.Add(new TrodelniNapad() { KazenskeTocke = 1, CasIzvedbe = 55 });

            // Izpišimo vrstni red
            Console.WriteLine();
            Console.WriteLine($"Ekipe članic so se razvrstile takole:");
            int count = 1;
            foreach (var ekipa in fisPrvenstvo.VrstniRed(KategorijaEnum.Člani, Spol.Ženske))
            {
                Console.WriteLine($"{count}. mesto: {ekipa.Ime}");
                count++;
            }
            
            Console.WriteLine();
            Console.WriteLine($"Ekipe članov pa takole:");
            count = 1;
            foreach (var ekipa in fisPrvenstvo.VrstniRed(KategorijaEnum.Člani, Spol.Moški))
            {
                Console.WriteLine($"{count}. mesto: {ekipa.Ime}");
                count++;
            }
            
            Console.WriteLine();
            Console.WriteLine($"Najmanj kazenskih točk so imele ekipe:");
            count = 1;
            foreach (var ekipa in NajmanjKazenskihTočk(fisPrvenstvo))
            {
                Console.WriteLine($"{count}. mesto: {ekipa.Ime}");
                count++;
            }

            // Paralelna izvedba (naloga 8)
            ParallelOptions po = new ParallelOptions();
            po.MaxDegreeOfParallelism = 2;

            Parallel.ForEach(katClani.Ekipe,
                po,
                ekipa =>
                {
                    Console.WriteLine($"Na delu je ekipa {ekipa.Ime}");
                    ((TrodelniNapad)(ekipa.Discipline.Single(disc => disc is TrodelniNapad))).Izvedi();
                    Console.WriteLine($"Ekipa {ekipa.Ime} je zaključila z delom!");
                });

            Console.ReadLine();
        }

        public static List<Ekipa> NajmanjKazenskihTočk(Tekmovanje tekmovanje)
        {
            return tekmovanje.Kategorije
                        .SelectMany(kat => kat.Ekipe) // Vse sezname ekip zapišemo v en seznam
                        .OrderBy(ekipa => ekipa.Discipline.Sum(disc => disc.KazenskeTocke)) // za vsako ekipo seštejemo kazenske točke
                        .Take(3) // vzamemo prve tri
                        .ToList();
        }
    }
}
