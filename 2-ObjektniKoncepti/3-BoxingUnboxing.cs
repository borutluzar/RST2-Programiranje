using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti
{
    /// <summary>
    /// Arh, Q13
    /// 'Boxing' je proces preoblikovanja ('cast') vrednostnega tipa v referenčnega
    /// 
    /// ! Oba procesa sta počasna, zato se odsvetuje njuno izvajanje na veliki količini objektov!
    /// Več o uporabi si bomo ogledali v razdelku o vmesnikih
    /// </summary>
    static class BoxingUnboxing
    {
        public static void TestBoxing()
        {
            
            int valType = 1;

            // Boxing - tipa ni potrebno zapisati eksplicitno            
            object refType = valType;

            Console.WriteLine($"1. valType={valType}, refType={refType}");
            Console.WriteLine();
                        
            // Ob predelavi se ustvari kopija vrednosti
            valType = 2;
            Console.WriteLine($"2. valType={valType}, refType={refType}");
            Console.WriteLine();
                        
            // Unboxing - tip moramo povedati eksplicitno
            valType = (int)refType;

            Console.WriteLine($"3. valType={valType}, refType={refType}");
            Console.WriteLine();

            // Če uporabimo napačno preoblikovanje, se ob izvajanju sproži napaka
            object refType2 = 3;
            //int l = (int)refType2;
            //int l = (long)refType2;
            long l = (int)refType2;

            Console.WriteLine($"4. valType={l}, refType={refType2}");
            Console.WriteLine();
        }
    }
}
