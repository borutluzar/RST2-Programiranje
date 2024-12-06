using CommonFunctions;
using DesignPatterns.StrategyPart3;

namespace Generics
{
    enum GenericsSection
    {
        Basics = 1,
        GenericClass = 2,
        GenericMethod = 3,
        GenericExample = 4
    }

    /// <summary>
    /// Generiki (to so lahko razredi, strukture, vmesniki ali metode) 
    /// nam omogočajo definiranje enakega obnašanja za različne tipe,
    /// pri čemer jim tipe podamo kot parametre (v kljukastih oklepajih).
    /// Arh, Q39
    /// </summary>
    class Program
    {
        static void Main()
        {
            var section = InterfaceFunctions.ChooseSection<GenericsSection>();
            switch (section)
            {
                case GenericsSection.Basics:
                    {
                        // Definirajmo generični vmesnik IUnique za enolične zapise, ki mu bomo kot parameter
                        // podali tip, ki ga naj lastnost ID vmesnika vrača.

                        // Najprej naredimo instanco razreda Citizen
                        IUnique<long> obcan = new Citizen(0101023500891);
                        Console.WriteLine($"ID občana {nameof(obcan)} je tipa {obcan.ID.GetType()}");

                        // In še instanco razreda Student
                        IUnique<int> student = new Student(35230001);
                        Console.WriteLine($"ID študenta {nameof(student)} je tipa {student.ID.GetType()}");
                    }
                    break;
                case GenericsSection.GenericClass:
                    {
                        // Če imamo generični razred, instanci podamo tip šele ob kreiranju

                        Article<long, int, string> macola = new(11112222112121, 0, "Moja super vrednost!");
                        Console.WriteLine($"ID artikla {nameof(macola)} je tipa {macola.ID.GetType()}");

                        Article<int, int, string> sponka = new Article<int, int, string>(12423, 1, "Nimam nobene vrednosti :(");
                        Console.WriteLine($"ID artikla {nameof(sponka)} je tipa {sponka.ID.GetType()}");
                    }
                    break;
                case GenericsSection.GenericMethod:
                    {
                        // Podobno lahko ustvarimo generične metode.
                        // Že ves čas na primer uporabljamo metodo
                        // ChooseSection iz razreda InterfaceFunctions
                        // Več primerov v Arh, Q40
                        var choice = InterfaceFunctions.ChooseSection<GenericsSection>();                        
                        
                        // Ali pa razširitveno metodo WriteCollection
                        List<string> lstArticles = new List<string>() { "A", "B", "#", "D" };
                        var msg = lstArticles.WriteCollection();
                        Console.WriteLine(msg);
                    }
                    break;
                case GenericsSection.GenericExample:
                    {
                        ForeignSpeaker<SpeakForeignLanguageSoSo> negovorec = new ForeignSpeaker<SpeakForeignLanguageSoSo>();
                        negovorec.SpeakForeignGenericly();

                        break;
                    }
            }

            Console.WriteLine();
            Console.WriteLine("Končano");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// Generični vmesnik
    /// </summary>
    public interface IUnique<T>
    {
        public T ID { get; set; }
    }

    /// <summary>
    /// Razred, ki implementira vmesnik s tipom long
    /// </summary>
    public class Citizen : IUnique<long>
    {
        public Citizen(long emso)
        {
            ID = emso;
        }

        public long ID { get; set; }
    }

    /// <summary>
    /// Razred, ki implementira vmesnik s tipom int
    /// </summary>
    public class Student : IUnique<int>
    {
        public Student(int enrolmentNumber)
        {
            ID = enrolmentNumber;
        }

        public int ID { get; set; }
    }

    /// <summary>
    /// Lahko pa definiramo generični razred
    /// in instanci določimo tip šele takrat,
    /// ko jo kreiramo.
    /// </summary>
    public class Article<T, U, V> : IUnique<T>
    {
        public Article(T articleID, U key, V value)
        {
            ID = articleID;
            KeyValue = (key, value);
        }

        public T ID { get; set; }

        public (U Key, V Value) KeyValue { get; set; }

        public override string ToString()
        {
            return $"{ID}";
        }
    }


    public class ForeignSpeaker<T> where T : IForeignLanguageSpeaker, new()
    {
        public void SpeakForeignGenericly()
        {
            T objSpeaker = new T();
            objSpeaker.SpeakForeignLanguage("francoščina");
        }
    }
}