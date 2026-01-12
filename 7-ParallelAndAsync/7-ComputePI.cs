//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelAndAsync
{
    /// <summary>
    /// Primerjava izračuna vrednosti PI-ja na različne načine.
    /// Povzeto s strani: https://docs.microsoft.com/sl-si/samples/dotnet/samples/parallel-programming-compute-pi-cs/
    /// 
    /// Tip rezultata je načrtno spremenjen na decimal, pri double so razlike med posameznimi pristopi bolj očitne,
    /// pri decimal pa izginejo.
    /// </summary>
    class ComputePI
    {
        const int NumberOfSteps = 100_000_000;

        public static void ComputePITests()
        {
            Console.WriteLine("Function               | Elapsed Time     | Estimated Pi");
            Console.WriteLine("-----------------------------------------------------------------");

            // Izpišimo vrednost PI, ki je shranjena v konstanti
            Console.WriteLine($"Math.PI                | Elapsed Time     | {Math.PI}");

            // Zaporedni izračun z LINQ
            Time(SerialLinqPi, nameof(SerialLinqPi));
            // Paralelni izračun z LINQ
            Time(ParallelLinqPi, nameof(ParallelLinqPi));
            // Zaporedni izračun s for zanko
            Time(SerialPi, nameof(SerialPi));
            // Paralelno
            Time(ParallelPi, nameof(ParallelPi));
            // Z drobilcem
            Time(ParallelPartitionerPi, nameof(ParallelPartitionerPi));

            Console.WriteLine("Press any key to continue...");
        }

        /// <summary>Izpiše rezultat dane funkcije skupaj s časom izvajanja</summary>
        static void Time(
            Func<decimal> estimatePi,
            string function)
        {
            var sw = Stopwatch.StartNew();
            var pi = estimatePi();
            Console.WriteLine($"{function.PadRight(22)} | {sw.Elapsed} | {pi}");
        }

        /// <summary>Izračuna približek PI z LINQ implementacijo</summary>
        static decimal SerialLinqPi()
        {
            decimal step = 1.0m / (decimal)NumberOfSteps;
            return (from i in Enumerable.Range(0, NumberOfSteps)
                    let x = (i + 0.5m) * step
                    select 4.0m / (1.0m + x * x)).Sum() * step;
        }

        /// <summary>Izračuna približek PI s PLINQ implementacijo.</summary>
        static decimal ParallelLinqPi()
        {
            decimal step = 1.0m / (decimal)NumberOfSteps;
            return (from i in ParallelEnumerable.Range(0, NumberOfSteps)
                    let x = (i + 0.5m) * step
                    select 4.0m / (1.0m + x * x)).Sum() * step;
        }

        /// <summary>Izračuna približek PI s for zanko.</summary>
        static decimal SerialPi()
        {
            decimal sum = 0.0m;
            decimal step = 1.0m / (decimal)NumberOfSteps;
            for (int i = 0; i < NumberOfSteps; i++)
            {
                decimal x = (i + 0.5m) * step;
                sum += 4.0m / (1.0m + x * x);
            }
            return step * sum;
        }

        /// <summary>Izračuna približek PI s Parallel.For.</summary>
        static decimal ParallelPi()
        {
            decimal sum = 0.0m;
            decimal step = 1.0m / (decimal)NumberOfSteps;
            object monitor = new object();
            Parallel.For(0, NumberOfSteps, () => 0.0m, (i, state, local) =>
            {
                decimal x = (i + 0.5m) * step;
                return local + 4.0m / (1.0m + x * x);
            }, local => { lock (monitor) sum += local; });
            return step * sum;
        }

        /// <summary>
        /// Izračuna približek PI s Parallel.ForEach in delilcem intervala.
        /// </summary>
        static decimal ParallelPartitionerPi()
        {
            decimal sum = 0.0m;
            decimal step = 1.0m / (decimal)NumberOfSteps;
            object monitor = new object();
            Parallel.ForEach(Partitioner.Create(0, NumberOfSteps), () => 0.0m, (range, state, local) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    decimal x = (i + 0.5m) * step;
                    local += 4.0m / (1.0m + x * x);
                }
                return local;
            }, local => { lock (monitor) sum += local; });
            return step * sum;
        }
    }
}