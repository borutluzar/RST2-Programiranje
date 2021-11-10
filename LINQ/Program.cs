using CommonFunctions;
using System;
using System.Linq.Expressions;

namespace LINQ
{
    enum LINQSections
    {
        Basics,
        MethodSyntax,
        LambdaExpressions
    }

    class Program
    {
        static void Main()
        {
            switch (InterfaceFunctions.ChooseSection<LINQSections>())
            {
                case LINQSections.Basics:
                    {
                        Basics.BasicLINQSyntax();
                    }
                    break;
                case LINQSections.MethodSyntax:
                    {
                        Basics.MethodLINQSyntax();
                    }
                    break;
                case LINQSections.LambdaExpressions:
                    {
                        LambdaExpressions.LambdaTests();
                    }
                    break;
            }
            Console.ReadLine();
        }
    }
}
