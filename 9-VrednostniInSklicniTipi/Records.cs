namespace VrednostniInSklicniTipi
{
    /*
     * Record oziroma record class je sklicni tip, record struct pa vrednostni tip.
     *
     * Dokumentacija:
     * https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record
     * https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/with-expression
     */
    public static class Records
    {
        /// <summary>
        /// Dva različna objekta sta lahko enaka po vrednosti.
        /// </summary>
        public static void ExampleEquality()
        {
            // Definicija PointRecord je na koncu datoteke.
            // S kratkim zapisom dobimo konstruktor ter javni lastnosti X in Y.
            PointRecord p1 = new PointRecord(1, 2);
            PointRecord p2 = new PointRecord(1, 2);

            // Pri tem zapisu == primerja obe koordinati.
            // ReferenceEquals pa preveri, ali sklica kažeta na isti objekt.
            Console.WriteLine($"Enaki koordinati: {p1 == p2}"); // True
            Console.WriteLine($"Isti objekt: {object.ReferenceEquals(p1, p2)}"); // False

            // Prevajalnik pripravi tudi ToString, ki ga uporabi Console.WriteLine.
            Console.WriteLine(p1); // PointRecord { X = 1, Y = 2 }

            // Običajna prireditev pri sklicnem tipu kopira sklic.
            PointRecord p3 = p1;
            Console.WriteLine($"p1 in p3 sta isti objekt: {object.ReferenceEquals(p1, p3)}"); // True

            // Pri tej pozicijski definiciji record class imata lastnosti get in init.
            // Poznejše neposredno prirejanje zato ni dovoljeno.
            // Za prikaz napake prevajalnika lahko odkomentiramo naslednjo vrstico:
            // p1.X = 10;
        }

        /// <summary>
        /// with ustvari nov zapis; navadna prireditev ohrani isti objekt.
        /// </summary>
        public static void ExampleWith()
        {
            PointRecord p1 = new PointRecord(1, 2);
            PointRecord p3 = p1;

            // Kopiramo podatke v nov objekt in njegovo koordinato X nastavimo na 10.
            // Koordinata Y se ohrani. Prvotni objekt ostane nespremenjen.
            PointRecord p4 = p1 with { X = 10 };
            Console.WriteLine($"Prvotna točka: {p1}"); // PointRecord { X = 1, Y = 2 }
            Console.WriteLine($"Nova točka: {p4}");    // PointRecord { X = 10, Y = 2 }
            Console.WriteLine($"Enaki koordinati: {p1 == p4}"); // False
            Console.WriteLine($"Isti objekt: {object.ReferenceEquals(p1, p4)}"); // False

            // Tudi prazen with ustvari nov objekt, čeprav ne spremenimo podatkov.
            PointRecord copy = p1 with { };
            Console.WriteLine($"Prazni with – enaki podatki: {p1 == copy}"); // True
            Console.WriteLine($"Prazni with – isti objekt: {object.ReferenceEquals(p1, copy)}"); // False

            // Lastnosti in spremenljivka niso isto: spremenljivki p1 smemo prirediti
            // sklic na drug objekt. S tem ne spreminjamo prvotnega objekta.
            p1 = p1 with { X = 20 };
            Console.WriteLine($"p1 po novi prireditvi: {p1}"); // PointRecord { X = 20, Y = 2 }
            Console.WriteLine($"p3 še vedno kaže na prvotno točko: {p3}"); // PointRecord { X = 1, Y = 2 }
            Console.WriteLine($"p1 in p3 sta isti objekt: {object.ReferenceEquals(p1, p3)}"); // False
        }

        /// <summary>
        /// record struct kopira vrednost; readonly omeji spreminjanje lastnosti.
        /// </summary>
        public static void ExampleStructs()
        {
            PointValue v1 = new PointValue(1, 2);
            PointValue v2 = v1;

            // Pri vrednostnem tipu se kopira celotna vrednost.
            // Tudi record struct dobi operator == za primerjanje podatkov.
            Console.WriteLine($"Pred spremembo sta vrednosti enaki: {v1 == v2}"); // True

            // Pri tem pozicijskem record struct sta lastnosti X in Y spremenljivi.
            // Sprememba kopije ne spremeni prvotne vrednosti.
            v2.X = 10;
            Console.WriteLine($"v1.X = {v1.X}, v2.X = {v2.X}"); // v1.X = 1, v2.X = 10
            Console.WriteLine($"Po spremembi sta vrednosti enaki: {v1 == v2}"); // False

            // with lahko uporabimo tudi pri vrednostnem zapisu.
            PointValue v3 = v1 with { Y = 5 };
            Console.WriteLine($"v1.Y = {v1.Y}, v3.Y = {v3.Y}"); // v1.Y = 2, v3.Y = 5

            // readonly record struct ima v tem kratkem zapisu lastnosti get/init.
            ReadonlyPointValue r1 = new ReadonlyPointValue(1, 2);
            // r1.X = 10; // Napaka prevajalnika: poznejša prireditev ni dovoljena.

            // Lahko pa ustvarimo novo vrednost s spremenjeno koordinato.
            ReadonlyPointValue r2 = r1 with { X = 10 };
            Console.WriteLine($"r1.X = {r1.X}, r2.X = {r2.X}"); // r1.X = 1, r2.X = 10

            // Sama beseda record ne zagotavlja nespremenljivosti: v2.X smo spremenili.
            // ReferenceEquals tu ne uporabljamo: strukturi bi se zavili v objekta,
            // zato bi preverjali sklica na zaviti kopiji, ne enakosti vrednosti.
        }

        /// <summary>
        /// Nov zapis lahko še vedno deli spremenljive podatke s prvotnim zapisom.
        /// </summary>
        public static void ExampleShallowCopy()
        {
            // Lastnost Tags je sklic na tabelo. Tudi zanjo je prevajalnik pripravil init.
            TaggedPoint original = new TaggedPoint(1, 2, new string[] { "A" });

            // Tabeli imata enako vsebino, vendar gre za dve različni tabeli.
            TaggedPoint sameContents = new TaggedPoint(1, 2, new string[] { "A" });
            Console.WriteLine($"Zapisa z ločenima tabelama sta enaka: {original == sameContents}"); // False

            // Primerjanje zapisa sledi pravilom tipov njegovih podatkov.
            // Pri tabeli se privzeto primerjata sklica, ne njeni elementi.
            TaggedPoint equalCopy = original with { };
            Console.WriteLine($"Kopija z isto tabelo: {original == equalCopy}"); // True

            // Privzeto kopiranje z with je plitvo: nov zapis dobi isti sklic na tabelo.
            TaggedPoint shifted = original with { X = 10 };
            Console.WriteLine($"Isti zapis: {object.ReferenceEquals(original, shifted)}"); // False
            Console.WriteLine($"Ista tabela: {object.ReferenceEquals(original.Tags, shifted.Tags)}"); // True

            // Sklica Tags ne smemo zamenjati, element skupne tabele pa smemo spremeniti.
            // original.Tags = new string[] { "B" }; // Napaka prevajalnika zaradi init.
            shifted.Tags[0] = "B";
            Console.WriteLine($"Oznaka prvotne točke: {original.Tags[0]}"); // B
            Console.WriteLine($"Oznaka premaknjene točke: {shifted.Tags[0]}"); // B
            Console.WriteLine($"Oznaka v ločeni tabeli: {sameContents.Tags[0]}"); // A

            // Koordinati X sta neodvisni, saj sta bili kopirani kot vrednosti tipa int.
            Console.WriteLine($"original.X = {original.X}, shifted.X = {shifted.X}"); // 1, 10

            // init varuje pred zamenjavo sklica, ne pred spreminjanjem skupnega objekta.
            // Če želimo neodvisno tabelo, moramo ustvariti tudi novo tabelo.
        }
    }

    /// <summary>
    /// Sklicni zapis iz zapiskov. record je krajši zapis za record class.
    /// Prevajalnik pripravi konstruktor, lastnosti X in Y z get/init,
    /// primerjanje po vrednosti ter izpis podatkov.
    /// </summary>
    public record PointRecord(int X, int Y);

    /// <summary>
    /// Vrednostni zapis iz zapiskov. Lastnosti X in Y imata get/set.
    /// </summary>
    public record struct PointValue(int X, int Y);

    /// <summary>
    /// Vrednostni zapis z lastnostma get/init. Obe koordinati sta tipa int.
    /// </summary>
    public readonly record struct ReadonlyPointValue(int X, int Y);

    /// <summary>
    /// Sklicni zapis s tabelo za prikaz plitvega kopiranja in primerjanja tabel.
    /// </summary>
    public record TaggedPoint(int X, int Y, string[] Tags);
}
