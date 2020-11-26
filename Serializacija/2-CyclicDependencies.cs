using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Serializacija
{
    class CyclicDependencies
    {
        public static void CyclicSerialization()
        {
            // Ustvarimo tri vozlišča drevesa
            Node<string> root = new Node<string>() { Value = "startek" };
            Node<string> leftSon = new Node<string>() { Value = "levi sinek" };
            Node<string> rightSon = new Node<string>() { Value = "desni sinek" };

            // Ciklična odvisnost po tipih ni problematična.
            root.Left = leftSon;
            root.Right = rightSon;
            
            // Če pa dodamo ciklično odvisnost po instancah, naletimo na težavo.
            // Klicu serializacije moramo povedati, naj vodi reference na posamezne instance
            /*leftSon.Parent = root;
            rightSon.Parent = root;

            // S spodnjo nastavitvijo vsak objekt dobi svoj ID, po katerem ga serializator prepozna
            var serSettings = new DataContractSerializerSettings()
            {
                PreserveObjectReferences = true
            };*/

            SerializationBasics.SerializeObject<Node<string>>(root, "root.xml");

            Console.WriteLine("Objekt je uspešno serializiran.");

            // Naredimo še deserializacijo
            Node<string> newRoot = SerializationBasics.DeserializeObject<Node<string>>("root.xml");

            Console.WriteLine($"Objekt je uspešno deserializiran: {newRoot.Value}");

            // Podobno kot s cikli lahko upravljamo z instancami abstraktnih razredov ali vmesnikov
        }
    }

    /// <summary>
    /// Implementirajmo binarno drevo
    /// </summary>
    [DataContract]
    public class Node<T>
    {
        [DataMember]
        public T Value { get; set; }

        [DataMember]
        public Node<T> Left { get; set; }

        [DataMember]
        public Node<T> Right { get; set; }

        [DataMember]
        public Node<T> Parent { get; set; }

        private bool isLeaf;
        [DataMember]
        public bool IsLeaf 
        {
            get
            {
                return this.Left == null && this.Right == null;
            }
            // Set moramo implementirati za potrebe deserializacije
            private set
            {
                this.isLeaf = this.Left == null && this.Right == null;
            }
        }
    }
}
