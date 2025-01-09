using CommonFunctions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelAndAsync
{
    enum ConcurrentSection
    {
        Multithreaded = 1,
        Tasks = 2,
        TasksWithResult = 3,
        Timers = 4,
        PLINQ = 5,
        PLINQOrdered = 6,
        Async = 7,
        AsyncSeveral = 8,
        AsyncFiles = 9,
        AsyncFilesCancel = 10,
        AsyncFilesProgress = 11,
        Lock = 12,
        ComputePI = 13,
        BreakfastBad = 14,
        BreakfastGood = 15
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            switch (InterfaceFunctions.ChooseSection<ConcurrentSection>())
            {
                case ConcurrentSection.Multithreaded:
                    {
                        Multithreads.Multithreaded();
                    }
                    break;
                case ConcurrentSection.Tasks:
                    {
                        Multithreads.Tasks();
                    }
                    break;
                case ConcurrentSection.TasksWithResult:
                    {
                        Multithreads.TasksResult();
                    }
                    break;
                case ConcurrentSection.Timers:
                    {
                        Timers.TimersTest();
                    }
                    break;
                case ConcurrentSection.PLINQ:
                    {
                        ParallelLINQ.PLINQExample();
                    }
                    break;
                case ConcurrentSection.PLINQOrdered:
                    {
                        ParallelLINQ.PLINQExampleOrdered();
                    }
                    break;
                case ConcurrentSection.Async:
                    {
                        Asynchronous.AsyncTest(); // asinhrona funkcija
                        // Ker await ne blokira izvajalne niti, pred izračunom pridemo iz funkcije
                        Thread.Sleep(1_000);
                        Console.WriteLine("Smo na koncu primera!");
                        // Vrstni red čakanja na input ni popolnoma jasen...                        
                        //Thread.Sleep(5_000);
                        Console.ReadLine();
                    }
                    break;
                case ConcurrentSection.AsyncSeveral:
                    {
                        string keyword = "in";
                        Asynchronous.AsyncTestSeveral(keyword);
                        Console.WriteLine($"Program se vmes nadaljuje...");
                    }
                    break;
                case ConcurrentSection.AsyncFiles:
                    {
                        AsyncFiles.AsyncFilesTest();
                    }
                    break;
                case ConcurrentSection.AsyncFilesCancel:
                    {
                        AsyncFiles.AsyncFilesTestWithCancel();
                    }
                    break;
                case ConcurrentSection.AsyncFilesProgress:
                    {
                        AsyncFiles.AsyncFilesTestWithCancelAndProgress();
                    }
                    break;
                case ConcurrentSection.Lock:
                    {
                        LockAndMonitor.LockExample();
                    }
                    break;
                case ConcurrentSection.ComputePI:
                    {
                        ComputePI.ComputePITests();
                    }
                    break;
                case ConcurrentSection.BreakfastBad:
                    {
                        PrepareBreakfastAsync.BreakfastBadExample();
                    }
                    break;
                case ConcurrentSection.BreakfastGood:
                    {
                        //PrepareBreakfastAsync.BreakfastBadExample();
                        await PrepareBreakfastAsync.BreakfastGoodExample();
                    }
                    break;
            }
            Console.ReadLine();
        }
    }
}
