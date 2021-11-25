using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Serializacija
{
    /// <summary>
    /// Arh, Q73
    /// Serializacijo uporabljamo, ko želimo shraniti instance, 
    /// ki nastopajo v našem programu v datoteko (ali nek drug izhodni tok).
    /// Z njeno pomočjo se ustvari zapis, iz katerega znamo pri postopku
    /// deserializacije ponovno ustvariti instanco z identičnimi lastnostmi
    /// in jo uporabljati naprej v programu.
    /// 
    /// Najdemo lahko veliko primerov uporabe, recimo:
    /// - shranjevanje trenutnega stanja programa - ko program teče dlje časa, 
    ///     npr. na super-računalniku več tednov, tam pa se pogosto dogajajo izklopi in posodobitve,
    ///     želimo da se naš proces ustrezno zamrzne in nadaljuje od točke prekinitve
    /// - prevelika poraba spomina - pri požrešnih programih lahko hitro porabimo RAM, ki je na voljo,
    ///     zato je v teh primerih smiselno podatke začasno shraniti v datoteko in jih nato iz nje brati posamič.
    ///     
    /// Priporočen tip serializacije v .Net je trenutno Data Contract Serialization.
    /// Za njeno uporabo moramo tipe označiti z atributom DataContract (uporaba System.Runtime.Serialization).
    /// V serializacijo so nato vključene lastnosti in polja, ki imajo atribut DataMember.
    /// Serializirani so lahko tako javni kot bolj zasebni tipi.
    /// </summary>
    public class SerializationBasics
    {
        public static void Serialize()
        {
            // Ustvarimo dva študenta
            Student marko = new Student("Marko")
            {
                StudentID = 35002020,
                LastName = "Markovič",
                EnrollmentYear = 2020
            };

            Student mirko = new Student("Mirko")
            {
                StudentID = 35002120,
                LastName = "Mirkovič",
                EnrollmentYear = 2020
            };

            // Serializirajmo ju
            SerializeObject<Student>(marko, "marko.xml");
            SerializeObject<Student>(mirko, "mirko.xml");

            Console.WriteLine("Serializacija je bila uspešna!");

            /* V datoteki marko.txt dobimo naslednji xml zapis
            <?xml version="1.0"?>
            <Student xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/Serializacija">
                <FirstName>Marko</FirstName>
                <LastName>Markovič</LastName>
                <StudentID>35002020</StudentID>
            </Student>
            */


            // Če želimo večjo kontrolo nad serializiranim objektom, 
            // uporabimo XML serializacijo, ki nam jo omogoči XmlSerializer.
            // V tem primeru se ciklične odvisnosti ne rešijo tako elegantno kot v našem naslednjem primeru.

            // Obstajajo tudi drugi načini serializacije, npr. binarna (razred mora imeti atribut Serializable)
            // ali pa v JSON (to omogoča NuGet paket NewtonSoft) in druge.
        }

        public static void SerializeObject<T>(T obj, string fileName, DataContractSerializerSettings settings = null)
        {
            var serObject = new DataContractSerializer(typeof(T), settings);

            using var sw = File.Create(fileName);
            serObject.WriteObject(sw, obj);
        }


        public static void Deserialize()
        {
            // Ustvarimo dva študenta, ki ju preberimo iz datotek
            Student janez = DeserializeObject<Student>("marko.xml");
            Student timotej = DeserializeObject<Student>("mirko.xml");

            Console.WriteLine($"Študent janez ima lastnosti: {janez.StudentID}, {janez.LastName}, {janez.EnrollmentYear}");
            Console.WriteLine($"Študent timotej ima lastnosti: {timotej.StudentID}, {timotej.LastName}, {timotej.EnrollmentYear}");
            // First name preverimo v debug načinu, ker ni javna lastnost


            Console.WriteLine("Deserializacija je bila uspešna!");
        }

        /// <summary>
        /// Prebere lastnosti objekta iz datoteke in jih zapiše v instanco
        /// </summary>
        public static T DeserializeObject<T>(string fileName)
        {
            var serObject = new DataContractSerializer(typeof(T));

            using var sw = File.OpenRead(fileName);
            T obj = (T)serObject.ReadObject(sw);
            return obj;
        }
    }

    [DataContract] // Obvezen atribut za serializacijo!
    public class Student
    {
        public Student(string firstName)
        {
            this.FirstName = firstName;
        }

        [DataMember] // Obvezen atribut, če želimo lastnost serializirati
        public int StudentID { get; set; }

        [DataMember] // Obvezen atribut, če želimo lastnost serializirati
        protected string FirstName { get; set; }

        [DataMember] // Obvezen atribut, če želimo lastnost serializirati
        public string LastName { get; set; }

        // Brez atributa, zato lastnost ne bo udeležena v serializaciji
        public int EnrollmentYear { get; set; }
    }
}

