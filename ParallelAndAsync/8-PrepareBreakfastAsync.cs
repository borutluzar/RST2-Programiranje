using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ParallelAndAsync
{
    /// <summary>
    /// Primer iz strani https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
    /// 
    /// Priprava zajtrka je lep primer asinhronega dela, ki ni paralelno.
    /// Našega procesa ne razdelimo na nekaj nekaj ekvivalentnih delov 
    /// in poskusimo izvajati čim več teh delov hkrati, ampak gre za skupek opravil,
    /// ki se lahko izvajajo hkrati, obenem pa tudi sledijo določenemu zaporedju.
    /// Npr. preden začnemo s peko omlete, moramo segreti ponev. 
    /// Medtem ko se ponev greje, lahko narežemo kruh in vzamemo iz hladilnika salamo.
    /// Kruh damo v opekač in ko čakamo nanj, lahko umešamo jajca...
    /// </summary>
    class PrepareBreakfastAsync
    {
        // Pripravimo si prazne razrede za objekte, ki jih bomo pripravili za zajtrk
        public class Coffee { }
        public class Egg { }
        public class Bacon { }
        public class Toast { }
        public class Juice { }


        // Začnimo z običajnim primerom - primer slabe prakse
        public static void BreakfastBadExample()
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.WriteLine("Pripravimo si dober zajtrk brez asinhronih pristopov!");

            Coffee cup = PourCoffee();
            Console.WriteLine("\n----> coffee is ready");

            Egg eggs = FryEggs(2);
            Console.WriteLine("\n----> eggs are ready");

            Bacon bacon = FryBacon(3);
            Console.WriteLine("\n----> bacon is ready");

            Toast toast = ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("\n----> toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine("\n----> oj is ready");
            Console.WriteLine("\n----> ----> Breakfast is ready!");

            Console.WriteLine($"Zajtrk smo si pripravili v {sw.Elapsed.TotalSeconds} sekundah!");
        }                

        private static Juice PourOJ()
        {
            Console.WriteLine("\tJuice: \tPouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine("\tJam: \tPutting jam on the toast");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine("\tButter: \tPutting butter on the toast");

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("\tToast: \tPutting a slice of bread in the toaster");
            }
            Console.WriteLine("\tToast: \tStart toasting...");
            Task.Delay(3000).Wait();
            Console.WriteLine("\tToast: \tRemove toast from toaster");

            return new Toast();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"\tBacon: \tputting {slices} slices of bacon in the pan");
            Console.WriteLine("\tBacon: \tcooking first side of bacon...");
            Task.Delay(3000).Wait();
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine($"\tBacon: \tflipping slice #{slice} of bacon");
            }
            Console.WriteLine("\tBacon: \tcooking the second side of bacon...");
            Task.Delay(3000).Wait();
            Console.WriteLine("\tBacon: \tPut bacon on plate");

            return new Bacon();
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("\tEggs: \tWarming the egg pan...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"\tEggs: \tcracking {howMany} eggs");
            Console.WriteLine("\tEggs: \tcooking the eggs ...");
            Task.Delay(3000).Wait();
            Console.WriteLine("\tEggs: \tPut eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("\tCoffee: \tPouring coffee");
            return new Coffee();
        }



        // Asinhrona koda izgleda takole
        public static async Task BreakfastGoodExample()
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.WriteLine("Pripravimo si dober zajtrk asinhrono!");


            Coffee cup = PourCoffee();
            Console.WriteLine("\n----> coffee is ready");

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);

            var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };
            while (breakfastTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(breakfastTasks);
                if (finishedTask == eggsTask)
                {
                    Console.WriteLine("\n----> eggs are ready");
                }
                else if (finishedTask == baconTask)
                {
                    Console.WriteLine("\n----> bacon is ready");
                }
                else if (finishedTask == toastTask)
                {
                    Console.WriteLine("\n----> toast is ready");
                }
                breakfastTasks.Remove(finishedTask);
            }

            Juice oj = PourOJ();
            Console.WriteLine("\n----> oj is ready");
            Console.WriteLine("\n----> ----> Breakfast is ready!");

            Console.WriteLine($"Zajtrk smo si pripravili v {sw.Elapsed.TotalSeconds} sekundah!");
        }

        static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("\tToast: \tPutting a slice of bread in the toaster");
            }
            Console.WriteLine("\tToast: \tStart toasting...");
            await Task.Delay(3000);
            Console.WriteLine("\tToast: \tRemove toast from toaster");

            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"\tBacon: \tputting {slices} slices of bacon in the pan");
            Console.WriteLine("\tBacon: \tcooking first side of bacon...");
            await Task.Delay(3000);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine($"\tBacon: \tflipping slice #{slice} of bacon");
            }
            Console.WriteLine("\tBacon: \tcooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("\tBacon: \tPut bacon on plate");

            return new Bacon();
        }

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("\tEggs: \tWarming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"\tEggs: \tcracking {howMany} eggs");
            Console.WriteLine("\tEggs: \tcooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("\tEggs: \tPut eggs on plate");

            return new Egg();
        }
    }
}
