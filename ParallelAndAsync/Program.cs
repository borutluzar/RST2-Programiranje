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
        Monitor
    }

    class Program
    {
        static void Main(string[] args)
        {
            Section section = Section.Async;

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
                        string keyword = "Janez";
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
            }
            Console.ReadLine();

        }
    }
}
