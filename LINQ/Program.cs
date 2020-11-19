using System;
using System.Linq.Expressions;

namespace LINQ
{
    enum Section
    {
        Basics,
        MethodSyntax,
        LambdaExpressions
    }

    class Program
    {
        static void Main(string[] args)
        {
            Section section = Section.LambdaExpressions;

            Console.WriteLine();
            switch (section)
            {
                case Section.Basics:
                    {
                        Basics.BasicLINQSyntax();
                    }
                    break;
                case Section.MethodSyntax:
                    {
                        Basics.MethodLINQSyntax();
                    }
                    break;
                case Section.LambdaExpressions:
                    {
                        LambdaExpressions.LambdaTests();
                    }
                    break;
            }
            Console.ReadLine();
        }
    }
}
