using System;

namespace CommonFunctions
{
    public static class InterfaceFunctions
    {
        /// <summary>
        /// Funkcija izpiše možne sekcije in prebere izbiro uporabnika.
        /// Tip enumeracije ji podamo preko parametra generičnega tipa T.
        /// </summary>
        public static T ChooseSection<T>()
        {
            // Izpis sekcij za izbiro 
            int i = 1;
            Console.WriteLine("--\t--\t--\t--");
            Console.WriteLine("Example sections:\n");
            foreach (var section in Enum.GetValues(typeof(T)))
            {
                Console.WriteLine($"{i}. {section}");
                i++;
            }
            Console.WriteLine("\n--\t--\t--\t--");
            Console.Write("Choose example section to run: ");

            string input = Console.ReadLine();
            bool isFormatCorrect = int.TryParse(input, out int chosen);
            if (!isFormatCorrect)
            {
                Console.WriteLine($"\n The input {input} is not an integer! The execution is stopped.");
                return default;
            }
            else if (chosen <= 0 || chosen > i)
            {
                Console.WriteLine($"\n There is no Section {input}! The execution is stopped.");
                return default;
            }

            Console.Write("\n");
            Console.WriteLine($"Running example section {chosen}...");
            Console.Write("\n\n");
            
            // Pretvorba (cast) iz int nazaj v enumeracijo ni možna 
            // neposredno iz int (saj je enumeracija lahko kakega drugega
            // celoštevilskega tipa), zato chosen najprej pretvorimo v
            // object in šele nato v T.
            return (T)(object)chosen;
        }
    }
}
