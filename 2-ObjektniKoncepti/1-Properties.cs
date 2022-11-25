using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti
{
    // Več o lastnostih:
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#147-properties
    // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties
    public static class Properties
    {
        /// <summary>
        /// Metoda za prikaz primerov uporabe lastnosti
        /// </summary>
        public static void CheckProperties()
        {
            // Ustvarimo novo instanco objekta
            PropertiesExample pe = new PropertiesExample();
            
            // Vrednost polja field lahko beremo in nastavljamo brez omejitev
            uint x = pe.field;
            pe.field = 22;
            Console.WriteLine();
            Console.WriteLine($"Vrednost x={x}, vrednost polja field je {pe.field}");

            
            // Vrednost lastnosti lahko nastavimo samo,
            // če imamo ustrezen nivo dostopa
            pe.Property = 101;
            Console.WriteLine();
            Console.WriteLine($"Vrednost Property={pe.Property}");
            
            
            // Vrednosti samodejno implementirane lastnosti lahko
            // nastavljamo in beremo na enak način kot običajno
            // implementirano metodo
            //pe.AutoImplementedProperty = 14;
            Console.WriteLine();
            Console.WriteLine($"Vrednost AutoImplementedProperty={pe.AutoImplementedProperty}");
            Console.WriteLine();
            pe.MethodExample();
            Console.WriteLine($"Vrednost po izvedbi metode AutoImplementedProperty={pe.AutoImplementedProperty}");
            
            
            // Če se odločimo, da vrednosti lastnosti zagotovo
            // ne bomo spreminjali, izpustimo definicijo 'set'
            //pe.ReadOnlyAutoImplementedProperty = 122;
            Console.WriteLine();
            Console.WriteLine($"Vrednost ReadOnlyAutoImplementedProperty={pe.ReadOnlyAutoImplementedProperty}");            
        }
    }

    /// <summary>
    /// Arh, Q34
    /// Zakaj uporabljati lastnosti namesto javnih spremenljivk?
    /// </summary>
    public class PropertiesExample
    {
        // Polje lahko zaščitimo samo z zmanjšanjem vidnosti, 
        // vendar s tem zmanjšamo dostop branju in pisanju hkrati
        public uint field = 13;

        // Pri lastnostih imamo popolno kontrolo
        private uint propertyValue = 21;

        public uint Property
        {
            get
            {
                return propertyValue;
            }

            // Pri nastavljanju in branju lahko celo izvajamo kontrolo
            // s pisanjem dodatne kode
            // Več o nivojih dostopov lahko preberete tukaj:
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/accessibility-levels
            internal set
            {
                if (value % 2 == 0)
                    throw new ArgumentException("The provided value is not odd!");

                // "value" predstavlja vrednost, ki jo podamo lastnosti ob nastavljanju
                propertyValue = value;
            }
        }

        /// <summary>
        /// Uporabimo lahko tudi samodejno implementacijo lastnosti.
        /// V tem primeru prevajalnik za vrednost v ozadju pripravi polje, ki nosi podatek
        /// Pri tovrstnih lastnostih ne moremo dodati logike ob izvajanju. Če se takšna potreba
        /// izkaže, lastnost lahko naknadno spremenimo v široko obliko.
        /// 
        /// Če omejimo vidnost nastavljanja na private, imamo tri možnosti: 
        /// - neposredno nastavitev vrednosti za lastnostjo
        /// - nastavitev v konstruktorju
        /// - nastavitev v lokalni metodi
        /// </summary>
        public uint AutoImplementedProperty { get; private set; } = 43;

        public PropertiesExample()
        {
            //this.AutoImplementedProperty = 44;
            this.ReadOnlyAutoImplementedProperty = 12;
        }

        public void MethodExample()
        {
            this.AutoImplementedProperty = 56;
            //this.ReadOnlyAutoImplementedProperty = 67;
        }

        /// <summary>
        /// Ko želimo definirati lastnost, katere vrednosti ne želimo spreminjati,
        /// potem izpustimo določilo 'set' in vrednost nastavimo takoj ob definiranju ali v konstruktorju
        /// </summary>
        public uint ReadOnlyAutoImplementedProperty { get; } = 57;
    }

}
