using CommonFunctions;

namespace VrednostniInSklicniTipi
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (InterfaceFunctions.ChooseSection<ValueAndReferenceTypes>())
            {
                case ValueAndReferenceTypes.Basics:
                    { // Ogledamo si razliko med vrednostnimi in sklicnimi tipi

                        // Pri vrednostnih tipih se vrednosti kopirajo 
                        int x = 1;
                        int y = x; // y dobi vrednost x, torej 1
                        x = 2; // x dobi vrednost 2, y pa ohrani 1
                        Console.WriteLine($"x = {x}, y = {y}");

                        // Pri sklicnih se referenca ohranja
                        List<int> z = new List<int>() { 1, 2, 3 };
                        List<int> w = z;
                        z.Add(0); // z in w kažeta na isti seznam, v obeh bo 0.
                        Console.WriteLine($"Velikost z: {z.Count}, w: {w.Count}");

                        // Dokler je ne spremenimo
                        z = new List<int>() { 5, 6, 7 };
                        // z ima novo referenco, w-ju ostane stara
                        Console.WriteLine($"Velikost z: {z.Count}, w: {w.Count}");
                    }
                    break;
                case ValueAndReferenceTypes.DefaultValue:
                    {
                        // Spremenljivka vrednostnega tipa vedno ima neko vrednost
                        // Če ji je ne predpišemo, dobi privzeto vrednost
                        // Nastavimo jo z rezervirano besedo default
                        int i = default; // Privzeta vrednost za celoštevilske tipe je 0

                        // Pri sklicnih tipih se vrednost samodejno ne nastavi
                        // Spremenljivke do takrat ne kažejo na nobeno referenco
                        // in to označujemo z null, podobno to tudi nastavimo
                        object j = null;
                    }
                    break;
                case ValueAndReferenceTypes.StructsAndClasses:
                    {
                        // Začnimo s primerom sklicnega tipa
                        Rectangle_Class rect_ref = new Rectangle_Class();

                        Console.WriteLine($"Sklicni tip na začetku:");
                        Console.WriteLine($"Stranica a = {rect_ref.a}");
                        Console.WriteLine($"Stranica b = {rect_ref.b}");

                        // Pokličemo funkcijo, ki spremeni vrednosti podanega pravokotnika
                        IncreaseRectangle(rect_ref, 1);

                        // Spremembe se ohranijo, saj smo vrednosti spreminjali
                        // objektu, na katerega kaže referenca
                        Console.WriteLine($"\nPo povečanju izven funkcije:");
                        Console.WriteLine($"Stranica a = {rect_ref.a}");
                        Console.WriteLine($"Stranica b = {rect_ref.b}");


                        // In še primer vrednostnega tipa
                        Rectangle_Struct rect_val = new Rectangle_Struct();

                        Console.WriteLine($"\nVrednostni tip na začetku:");
                        Console.WriteLine($"Stranica a = {rect_val.a}");
                        Console.WriteLine($"Stranica b = {rect_val.b}");
                                              

                        // Pokličemo funkcijo, ki spremeni vrednosti podanega pravokotnika
                        IncreaseRectangle(rect_val, 1);

                        // Spremembe se ne ohranijo, saj smo vrednosti spreminjali
                        // novemu objektu, ki je bil kopija podanega
                        Console.WriteLine($"\nPo povečanju izven funkcije:");
                        Console.WriteLine($"Stranica a = {rect_val.a}");
                        Console.WriteLine($"Stranica b = {rect_val.b}");
                    }
                    break;
                case ValueAndReferenceTypes.BoxingAndUnboxing:
                    {
                        BoxingUnboxing.ExampleBoxing();
                    }
                    break;
            }
            Console.Read();
        }

        private enum ValueAndReferenceTypes
        {
            Basics = 1,
            DefaultValue = 2,
            StructsAndClasses = 3,
            BoxingAndUnboxing = 4
        }

        public static void IncreaseRectangle(Rectangle_Class rect, int increaseBy)
        {
            rect.a = rect.a + increaseBy;
            rect.b = rect.b + increaseBy;
            Console.WriteLine($"\nPo povečanju v funkciji:");
            Console.WriteLine($"Stranica a = {rect.a}");
            Console.WriteLine($"Stranica b = {rect.b}");
        }

        public static void IncreaseRectangle(Rectangle_Struct rect, int increaseBy)
        {
            rect.a = rect.a + increaseBy;
            rect.b = rect.b + increaseBy;
            Console.WriteLine($"\nPo povečanju v funkciji:");
            Console.WriteLine($"Stranica a = {rect.a}");
            Console.WriteLine($"Stranica b = {rect.b}");
        }
    }    
}

