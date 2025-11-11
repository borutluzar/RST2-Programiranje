namespace VrednostniInSklicniTipi
{
    /// <summary>
    /// Arh, Q13
    /// 'Boxing' (mi bomo to prevajali kot 'zavijanje') je proces preoblikovanja ('cast') vrednostnega tipa v sklicnega.
    /// Obratni proces je 'unboxing' (oz. 'odvijanje').
    /// 
    /// ! Oba procesa sta počasna, zato se odsvetuje njuno izvajanje na veliki količini objektov!
    /// Več o uporabi si bomo ogledali v razdelku o vmesnikih
    /// </summary>
    static class BoxingUnboxing
    {
        public static void ExampleBoxing()
        {
            // Pripravimo si spremenljivko vrednostnega tipa
            int valType = 1;

            // Boxing - tipa ni treba zapisati eksplicitno
            object refType1 = valType;
            object refType2 = valType;

            Console.WriteLine($"1. valType={valType}, refType1={refType1}, refType2={refType2}");
            Console.WriteLine();
                        
            // Ob predelavi se ustvari kopija vrednosti
            valType = 2;
            refType2 = valType;
            Console.WriteLine($"Predelamo vrednost {nameof(valType)} v 2, {nameof(refType1)} pa pustimo.");
            Console.WriteLine($"2. valType={valType}, refType1={refType1}, refType2={refType2}");
            Console.WriteLine();

            /*
             * Začetek:
             * valType = 1; refType = valType
             * 1. valType -> [ 1 ]_a <- refType
             * 
             * Vrednostnemu tipu spremenimo vrednost => Ustvarimo novo kopijo za hrambo vrednosti
             * valType = 2
             * 2. valType -> [ 2 ]_b  |  [ 1 ]_a <- refType
             * 
            */

            // Unboxing - tip moramo povedati eksplicitno
            valType = (int)refType1;

            Console.WriteLine($"3. valType={valType}, refType1={refType1} , refType2={refType2}");
            Console.WriteLine();

            // Če uporabimo napačno preoblikovanje, se ob izvajanju sproži napaka
            object refType3 = 3;
            int l = (int)refType3;
            //int l = (long)refType2;
            //long l = (long)refType2; // Runtime exception - najprej moramo pretvoriti v int
            //long l = (int)refType2;

            Console.WriteLine($"4. valType={l}, refType={refType3}");
            Console.WriteLine();


            // Tip objekta lahko preverimo s funkcijo GetType()
            Console.WriteLine($"Tip objekta {nameof(refType1)}: " +
                $"{refType3.GetType().Name}");
            Console.WriteLine();

        }
    }
}
