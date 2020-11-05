using System;
using System.Collections.Generic;
using System.Text;

namespace ObjektniKoncepti
{
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
            // s pisanjem dodatne kode kodo
            private set
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
            //this.AutoImplementedProperty = 56;
            //this.ReadOnlyAutoImplementedProperty = 67;
        }

        /// <summary>
        /// Ko želimo definirati lastnost, katere vrednosti ne želimo spreminjati,
        /// potem izpustimo določilo 'set' in vrednost nastavimo takoj ob definiranju ali v konstruktorju
        /// </summary>
        public uint ReadOnlyAutoImplementedProperty { get; } = 57;
    }

}
