using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns
{

    /// <summary>
    /// Osnovni razred, iz katerega dedujejo vsi tipi računalnikov
    /// </summary>
    public abstract class Computer
    {
        // Bistvene lastnosti nastavimo na abstract,
        // da zagotovimo njihovo implementacijo v podrazredih
        public abstract string Processor { get; set; }
        public abstract string RAM { get; set; }
        public abstract string GraphicsCard { get; set; }
        public List<string> Ports { get; }

        internal Computer()
        {
            this.Ports = new List<string>();
        }

        public void DisplaySpecs()
        {
            Console.WriteLine($"Processor: {Processor}, RAM: {RAM}, " +
                $"GPU: {GraphicsCard}, Ports: {string.Join(", ", Ports)}");
        }
    }

    /// <summary>
    /// Konkreten podrazred, za katerega bomo kreirali instance
    /// </summary>
    public class Laptop : Computer
    {
        public override string Processor { get; set; }
        public override string RAM { get; set; }
        public override string GraphicsCard { get; set; }

        /// <summary>
        /// Konstruktor nastavimo na internal vidnost, 
        /// da preprečimo klice konstruktorja v drugih projektih,
        /// kjer se pričakuje uporaba factory-ja.
        /// </summary>
        internal Laptop() { }
    }

    /// <summary>
    /// Vmesnik, ki določi funkcije, 
    /// katere mora zagotoviti vsak builder.
    /// </summary>
    public interface IComputerBuilder
    {
        void SetProcessor(string processor);
        void SetRAM(string ram);
        void SetGraphics(string gpu);
        void AddPort(string port);
        Computer BuildComputer();
    }

    /// <summary>
    /// Builder razred, ki implementira funkcije 
    /// za kreiranje prenosnika.
    /// </summary>
    public class LaptopBuilder : IComputerBuilder
    {
        // Instanca prenosnika, ki jo dopolnjujemo po korakih
        private Laptop builderInstance = new Laptop();

        public void SetProcessor(string processor) 
            => builderInstance.Processor = processor;
        public void SetRAM(string ram) => builderInstance.RAM = ram;
        public void SetGraphics(string gpu) => builderInstance.GraphicsCard = gpu;
        public void AddPort(string port) => builderInstance.Ports.Add(port);

        public Computer BuildComputer()
        {
            Computer result = builderInstance;
            return result;
        }
    }

    /// <summary>
    /// Factory razred, ki poskrbi 
    /// za kreiranje instance izbranega tipa.
    /// </summary>
    public class ComputerFactory
    {
        private IComputerBuilder builder;

        public Computer? CreateComputer(ComputerType type)
        {
            Computer? instance = null;
            switch (type)
            {
                case ComputerType.OfficeLaptop:
                    builder = new LaptopBuilder();
                    instance = BuildOfficeLaptop();
                    break;
                case ComputerType.GamingLaptop:
                    builder = new LaptopBuilder();
                    instance = BuildGamingLaptop();
                    break;
            }
            return instance;
        }

        private Computer BuildGamingLaptop()
        {
            builder.SetProcessor("Intel i9");
            builder.SetRAM("32GB");
            builder.SetGraphics("NVIDIA RTX 4080");
            builder.AddPort("HDMI");
            builder.AddPort("USB-C");
            return builder.BuildComputer();
        }

        private Computer BuildOfficeLaptop()
        {
            builder.SetProcessor("Intel i5");
            builder.SetRAM("16GB");
            builder.SetGraphics("Integrated Graphics");
            builder.AddPort("USB-A");
            return builder.BuildComputer();
        }
    }

    public enum ComputerType
    {
        GamingLaptop = 1,
        OfficeLaptop = 2
    }
}
