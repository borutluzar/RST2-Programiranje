using System;

namespace ParallelAndAsync
{
    enum Section
    {
        Multithreaded,
        Tasks,
        TasksWithResult,
        Timers,
        PLINQ,
        PLINQOrdered,
        Async,
        AsyncSeveral,
        AsyncFiles,
        AsyncFilesCancel,
        AsyncFilesProgress,
        Lock,
        Monitor,
        ComputePI
    }

    class Program
    {
        static void Main(string[] args)
        {
            Section section = Section.ComputePI;

            Console.WriteLine();
            switch (section)
            {
                case Section.Multithreaded:
                    {
                        Multithreads.Multithreaded();
                    }
                    break;
                case Section.Tasks:
                    {
                        Multithreads.Tasks();
                    }
                    break;
                case Section.TasksWithResult:
                    {
                        Multithreads.TasksResult();
                    }
                    break;
                case Section.Timers:
                    {
                        Timers.TimersTest();
                    }
                    break;
                case Section.PLINQ:
                    {
                        ParallelLINQ.PLINQExample();
                    }
                    break;
                case Section.PLINQOrdered:
                    {
                        ParallelLINQ.PLINQExampleOrdered();
                    }
                    break;
                case Section.Async:
                    {
                        Asynchronous.AsyncTest();
                    }
                    break;
                case Section.AsyncSeveral:
                    {
                        string keyword = "fiš";
                        Asynchronous.AsyncTestSeveral(keyword);
                    }
                    break;
                case Section.AsyncFiles:
                    {
                        AsyncFiles.AsyncFilesTest();
                    }
                    break;
                case Section.AsyncFilesCancel:
                    {
                        AsyncFiles.AsyncFilesTestWithCancel();
                    }
                    break;
                case Section.AsyncFilesProgress:
                    {
                        AsyncFiles.AsyncFilesTestWithCancelAndProgress();
                    }
                    break;
                case Section.Lock:
                    {
                        LockAndMonitor.LockExample();
                    }
                    break;
                case Section.Monitor:
                    {
                        LockAndMonitor.LockExample();
                    }
                    break;
                case Section.ComputePI:
                    {
                        ComputePI.ComputePITests();
                    }
                    break;
            }
            Console.ReadLine();

        }
    }
}
