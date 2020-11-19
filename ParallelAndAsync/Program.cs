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
    }

    class Program
    {
        static void Main(string[] args)
        {
            Section section = Section.PLINQOrdered;

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
            }
            Console.ReadLine();

        }
    }
}
